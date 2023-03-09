using Integrador.Common.Contracts;
using Integrador.Gerador.Worker.Infra;
using MassTransit;

namespace Integrador.Gerador.Worker;

internal class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBus _bus;
    private readonly HeartRateRepository _heartRateRepository;

    public Worker(ILogger<Worker> logger, IBus bus, HeartRateRepository heartRateRepository)
    {
        _logger = logger;
        _bus = bus;
        _heartRateRepository = heartRateRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting up!");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            IReadOnlyCollection<HeartRate> heartBeatArray 
                = await _heartRateRepository.GetNotIntegratedValues();

            if (heartBeatArray is { Count: 0 }) continue;

            // TODO: Implementar envio em lote direto
            // Qual o limite de tamanho de mensagens do broker?
            foreach (HeartRate heartBeat in heartBeatArray)
            {
                await _bus.Publish(new HeartRateToInsertMessage
                {
                    Id = heartBeat.Id,
                    PersonId = heartBeat.PersonId,
                    Timestamp = heartBeat.Timestamp,
                    Value = heartBeat.Reading
                }, stoppingToken);
            }
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _heartRateRepository.Dispose();

        return base.StopAsync(cancellationToken);
    }
}
