using Connector.Consumer.Worker.Infra.InfluxDb;
using Connector.Consumer.Worker.RegisterTemperatureBatch;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                busConfigurator.Host("localhost", "/", hostConfigurator =>
                {
                    hostConfigurator.Username("admin");
                    hostConfigurator.Password("EQ3MrrGBwn8bAgaUz9Hjb3LuvP");
                });

                busConfigurator.ConfigureEndpoints(busRegistrationContext);
            });
        });

        services.AddTransient<TemperatureRepository>();
    })
    .Build();

await host.RunAsync();
