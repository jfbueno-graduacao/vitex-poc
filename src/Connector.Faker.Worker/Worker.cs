using Connector.Faker.Worker.Model;
using Connector.Faker.Worker.Repositories;

namespace Connector.Faker.Worker;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly TemperatureRepository _temperatureRepository;
    private readonly PeopleRepository _peopleRepository;

    public Worker(
        ILogger<Worker> logger,
        TemperatureRepository temperatureRepository,
        PeopleRepository peopleRepository
    )
    {
        _logger = logger;
        _temperatureRepository = temperatureRepository;
        _peopleRepository = peopleRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IReadOnlyCollection<Person> people = await _peopleRepository.GetAll();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var timestamp = DateTime.UtcNow;

            // Gera uma nova leitura de temperatura por pessoa
            var readingsToInsert = people.Select(person =>
                new Temperature(person.Id, person.VariateTemperature(), timestamp)
            );

            await _temperatureRepository.Insert(readingsToInsert);

            await Task.Delay(60_000 / 2, stoppingToken); // 0.5 minutos
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _temperatureRepository.Dispose();
        _peopleRepository.Dispose();

        return base.StopAsync(cancellationToken);
    }
}
