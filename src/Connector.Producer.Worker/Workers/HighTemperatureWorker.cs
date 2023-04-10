using Connector.Producer.Worker.Infra.Database;
using Connector.Producer.Worker.Infra.MessageBus;

namespace Connector.Producer.Worker.Workers;

internal sealed class HighTemperatureWorker : BackgroundService
{
    private readonly HighTemperatureSharedState _sharedState;
    private readonly ILogger<HighTemperatureWorker> _logger;
    private readonly TemperatureBusProxy _messageBusProxy;
    private readonly TemperatureRepository _repository;

    public HighTemperatureWorker(
        ILogger<HighTemperatureWorker> logger,
        HighTemperatureSharedState sharedState,
        TemperatureBusProxy messageBusProxy,
        TemperatureRepository repository
    )
    {
        _sharedState = sharedState;
        _logger = logger;
        _messageBusProxy = messageBusProxy;
        _repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_sharedState.HasAny)
            {
                _logger.LogInformation(
                    "There's {count} people with high temperature now",
                    _sharedState.Count
                );

                var temperatureReadings =
                    await _repository.GetNotIntegratedValuesFromPeopleIds(_sharedState.Items);

                // Thread-safety check
                // The SharedState could be updated in between an executing of the default worker 
                // (which doesn't check the SharedState getting database itens and sending it)
                if (temperatureReadings.Any())
                    await _messageBusProxy.Publish(temperatureReadings, stoppingToken);

            }
            
            await Task.Delay(1_000, stoppingToken);
        }
    }

    public override void Dispose()
    { 
        _repository.Dispose();
        base.Dispose();
    }
}
