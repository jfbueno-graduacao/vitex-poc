using Integrador.Gerador.Worker;
using Integrador.Gerador.Worker.Infra;
using MassTransit;
using System.Reflection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
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
                busConfigurator.Host("localhost", "/", hostConfigurator =>
                {
                    hostConfigurator.Username("admin");
                    hostConfigurator.Password("EQ3MrrGBwn8bAgaUz9Hjb3LuvP");
                });

                busConfigurator.ConfigureEndpoints(busRegistrationContext);
            });
        });

        services.AddHostedService<Worker>();
        services.AddTransient<HeartRateRepository>();
    })
    .Build();

await host.RunAsync();
