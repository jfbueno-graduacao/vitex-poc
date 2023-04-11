using Connector.Common.MessageBus.Config;
using Connector.Producer.Worker;
using Connector.Producer.Worker.Infra.Database;
using Connector.Producer.Worker.Infra.MessageBus;
using Connector.Producer.Worker.Workers;
using MassTransit;
using System.Reflection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.SetInMemorySagaRepositoryProvider();

            var entryAssembly = Assembly.GetEntryAssembly();

            x.AddConsumers(entryAssembly);
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

                busConfigurator.ConfigureEndpoints(busRegistrationContext);

                busConfigurator.ConfigurePublish(pipeConfigurator =>
                {
                    pipeConfigurator.UseFogNodeMetadataHeaders(context.Configuration);
                });
            });
        });

        services.AddHostedService<DefaultWorker>();
        services.AddHostedService<HighTemperatureMonitorWorker>();
        services.AddHostedService<HighTemperatureWorker>();

        services.AddTransient<TemperatureRepository>();
        services.AddTransient<TemperatureBusProxy>();

        services.AddSingleton<HighTemperatureSharedState>();
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
