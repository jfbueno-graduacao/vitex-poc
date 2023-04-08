using Integrador.Produtor.Worker.Infra.Database;
using Integrador.Produtor.Worker.Infra.MessageBus;
using MassTransit;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace Integrador.Produtor.Worker.Workers;

internal sealed class DefaultWorker : BackgroundService
{
    private readonly ILogger<DefaultWorker> _logger;
    private readonly TemperatureBusProxy _messageBusProxy;
    private readonly TemperatureRepository _temperatureRepository;

    public DefaultWorker(
        ILogger<DefaultWorker> logger,
        TemperatureBusProxy messageBusProxy,
        TemperatureRepository temperatureRepository
    )
    {
        _logger = logger;
        _messageBusProxy = messageBusProxy;
        _temperatureRepository = temperatureRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            IReadOnlyCollection<Temperature> temperatureReadings
                = await _temperatureRepository.GetNotIntegratedValues();
            
            if (temperatureReadings.Any())
            {
                _logger.LogInformation(
                    "Integrating {count} temperature readings",
                    temperatureReadings.Count
                );

                await _messageBusProxy.Publish(temperatureReadings, stoppingToken);
            }

            await Task.Delay(60_000 * 1, stoppingToken);
        }
    }

    public override void Dispose()
    {
        _temperatureRepository.Dispose();
        base.Dispose();
    }
}