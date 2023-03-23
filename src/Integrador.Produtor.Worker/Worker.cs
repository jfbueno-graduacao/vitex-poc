using Integrador.Common.Contracts;
using Integrador.Produtor.Worker.Infra;
using MassTransit;

namespace Integrador.Produtor.Worker;

internal class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBus _bus;
    private readonly TemperatureRepository _temperatureRepository;

    public Worker(ILogger<Worker> logger, IBus bus, TemperatureRepository temperatureRepository)
    {
        _logger = logger;
        _bus = bus;
        _temperatureRepository = temperatureRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting up!");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            IReadOnlyCollection<Temperature> temperatureReadings 
                = await _temperatureRepository.GetNotIntegratedValues();

            if (temperatureReadings is { Count: 0 }) continue;

            // TODO: Implementar envio em lote direto
            // Qual o limite de tamanho de mensagens do broker?
            foreach (Temperature temperature in temperatureReadings)
            {
                await _bus.Publish(new TemperatureToInsertMessage
                {
                    Id = temperature.Id,
                    PersonId = temperature.PersonId,
                    Timestamp = temperature.Timestamp,
                    Value = temperature.Value
                }, stoppingToken);
            }
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _temperatureRepository.Dispose();

        return base.StopAsync(cancellationToken);
    }
}
