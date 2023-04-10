using Integrador.Common.MessageBus.Contracts;
using Integrador.Produtor.Worker.Infra.Database;
using MassTransit;

namespace Integrador.Produtor.Worker.Infra.MessageBus;

internal sealed class TemperatureBusProxy
{
    private readonly IBus _messageBus;

    public TemperatureBusProxy(IBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Publish(IEnumerable<Temperature> temperaturesToSend, CancellationToken cancellationToken)
    {
        // TODO: Implementar envio em lote direto
        // Qual o limite de tamanho de mensagens do broker?

        foreach (Temperature temperature in temperaturesToSend)
        {
            await _messageBus.Publish(new TemperatureToInsertMessage
            {
                Id = temperature.Id,
                PersonId = temperature.PersonId,
                Timestamp = temperature.Timestamp,
                Value = temperature.Value
            }, cancellationToken);
        }
    }
}
