using Integrador.Consumidor.Worker;
using Integrador.Consumidor.Worker.RegisterTemperature;
using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Reflection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.SetInMemorySagaRepositoryProvider();

            var entryAssembly = Assembly.GetEntryAssembly();
            
            x.AddConsumer<RegisterTemperatureConsumer, RegisterTemperatureConsumerDefinition>();

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
    })
    .Build();

await host.RunAsync();
