using Integrador.Produtor.Worker;
using Integrador.Produtor.Worker.Infra.Database;
using Integrador.Produtor.Worker.Infra.MessageBus;
using Integrador.Produtor.Worker.Workers;
using MassTransit;
using System.Reflection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var rabbitMqUser = context.Configuration.GetValue<string>(RabbitMqSettings.UserKey)
            ?? throw new InvalidOperationException("RabbitMqConfig User is not set");
        
        var rabbitMqPass = context.Configuration.GetValue<string>(RabbitMqSettings.PasswordKey)
            ?? throw new InvalidOperationException("RabbitMqConfig User is not set");

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
