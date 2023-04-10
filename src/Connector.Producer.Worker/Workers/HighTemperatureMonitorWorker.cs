using Connector.Producer.Worker.Infra.Database;

namespace Connector.Producer.Worker.Workers;

internal sealed class HighTemperatureMonitorWorker : BackgroundService
{
    private readonly HighTemperatureSharedState _sharedState;
    private readonly ILogger<HighTemperatureMonitorWorker> _logger;
    private readonly TemperatureRepository _repository;

    public HighTemperatureMonitorWorker(
        HighTemperatureSharedState sharedState,
        ILogger<HighTemperatureMonitorWorker> logger,
        TemperatureRepository repository
    )
    {
        _sharedState = sharedState;
        _logger = logger;
        _repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Checking people with high temperature");

            var idsWithHighTemperature = await _repository.ListPeopleWithHighTemperature();

            if (idsWithHighTemperature.Any())
            {
                _sharedState.SetItems(idsWithHighTemperature);

                _logger.LogInformation(
                    "There's {count} people with high temperature right now. " +
                    "Setting their ids to shared state",
                    idsWithHighTemperature.Count
                );
            }
            else
            {
                _sharedState.Clear();
            }
            
            await Task.Delay(2_000, stoppingToken);
        }
    }

    public override void Dispose()
    {
        _repository.Dispose();
        base.Dispose();
    }
}
