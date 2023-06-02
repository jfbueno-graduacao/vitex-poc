using Connector.Common.MessageBus.Config;
using Connector.Consumer.Worker.Infra.InfluxDb;
using Connector.Consumer.Worker.RegisterTemperatureBatch;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddOptions<InfluxDbSettings>()
            .Bind(context.Configuration.GetSection("InfluxDbConfig"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.SetInMemorySagaRepositoryProvider();

            var entryAssembly = Assembly.GetEntryAssembly();
            
            x.AddConsumer<RegisterTemperatureBatchConsumer, RegisterTemperatureBatchConsumerDefinition>();

            x.AddSagaStateMachines(entryAssembly);
            x.AddSagas(entryAssembly);
            x.AddActivities(entryAssembly);

            x.UsingRabbitMq((busRegistrationContext, busConfigurator) =>
            {
                var config = GetRabbitMqConfig(context.Configuration);

                busConfigurator.Host(config.host, "/", hostConfigurator =>
                {
                    hostConfigurator.Username(config.user);
                    hostConfigurator.Password(config.password);
                });

                // TODO: Use ILogger here
                const int prefetchCount = 210;
                Console.WriteLine($"Setting PrefetchCount to {prefetchCount}");
                busConfigurator.PrefetchCount = prefetchCount;

                const int concurrencyLimit = 150;
                Console.WriteLine($"Setting UseConcurrencyLimit to {concurrencyLimit}");
                busConfigurator.UseConcurrencyLimit(concurrencyLimit);

                busConfigurator.ConfigureEndpoints(busRegistrationContext);
            });
        });

        services.AddTransient<TemperatureRepository>();
    })
    .Build();

await host.RunAsync();

static (string host, string user, string password) GetRabbitMqConfig(IConfiguration configuration)
{
    var rabbitMqHost = configuration.GetValue<string>(RabbitMqSettings.HostKey)
                       ?? throw new InvalidOperationException("RabbitMqConfig Host is not set");

    var rabbitMqUser = configuration.GetValue<string>(RabbitMqSettings.UserKey)
                       ?? throw new InvalidOperationException("RabbitMqConfig User is not set");

    var rabbitMqPass = configuration.GetValue<string>(RabbitMqSettings.PasswordKey)
                       ?? throw new InvalidOperationException("RabbitMqConfig User is not set");

    return (rabbitMqHost, rabbitMqUser, rabbitMqPass);
}
