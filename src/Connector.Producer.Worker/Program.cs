using Connector.Producer.Worker;
using Connector.Producer.Worker.Infra.Database;
using Connector.Producer.Worker.Infra.MessageBus;
using Connector.Producer.Worker.Workers;
using MassTransit;
using System.Reflection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var rabbitMqUser = context.Configuration.GetValue<string>(RabbitMqSettings.UserKey)
            ?? throw new InvalidOperationException("RabbitMqConfig User is not set");
        
        var rabbitMqPass = context.Configuration.GetValue<string>(RabbitMqSettings.PasswordKey)
            ?? throw new InvalidOperationException("RabbitMqConfig User is not set");

        services.AddOptions<ConnectorSettings>()
            .Bind(context.Configuration.GetSection(ConnectorSettings.ConfigurationKey))
            .ValidateDataAnnotations()
            .ValidateOnStart();

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
                busConfigurator.Host("localhost", "/", hostConfigurator =>
                {
                    hostConfigurator.Username(rabbitMqUser);
                    hostConfigurator.Password(rabbitMqPass);
                });

                busConfigurator.ConfigureEndpoints(busRegistrationContext);
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
