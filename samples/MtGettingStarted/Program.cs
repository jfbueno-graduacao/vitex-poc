using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace MtGettingStarted
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        // By default, sagas are in-memory, but should be changed to a durable
                        // saga repository.
                        x.SetInMemorySagaRepositoryProvider();

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        x.AddSagaStateMachines(entryAssembly);
                        x.AddSagas(entryAssembly);
                        x.AddActivities(entryAssembly);

                        x.UsingRabbitMq((busRegistrationContext, config) => 
                        {
                            config.Host("localhost", "/", configurator => 
                            {
                                configurator.Username("admin");
                                configurator.Password("EQ3MrrGBwn8bAgaUz9Hjb3LuvP");
                            });

                            config.ConfigureEndpoints(busRegistrationContext);
                        });                        
                    });
                    
                    services.AddHostedService<Worker>();
                });
    }
}
