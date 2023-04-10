using Connector.Common.MessageBus.Contracts;
using Connector.Produtor.Worker.Infra.Database;
using MassTransit;

namespace Connector.Produtor.Worker.Infra.MessageBus;

internal sealed class TemperatureBusProxy
{
    private readonly IBus _messageBus;

    public TemperatureBusProxy(IBus messageBus) 
        => _messageBus = messageBus;

    public async Task Publish(IEnumerable<Temperature> temperaturesToSend, CancellationToken cancellationToken)
    {
        var temperatureBatch = temperaturesToSend.Select(t => new TemperatureBatchItem
        {
            Id = t.Id,
            PersonId = t.PersonId,
            Timestamp = t.Timestamp,
            Value = t.Value
        })
        .ToArray();

        await _messageBus.Publish(
            new TemperatureBatchToInsertMessage(temperatureBatch),
            cancellationToken
        );
    }
}
