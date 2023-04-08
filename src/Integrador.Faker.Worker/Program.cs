using Integrador.Faker.Worker;
using Integrador.Faker.Worker.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<TemperatureRepository>();
        services.AddTransient<PeopleRepository>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
