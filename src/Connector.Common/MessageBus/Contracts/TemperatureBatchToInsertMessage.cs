namespace Connector.Common.MessageBus.Contracts;

public sealed record TemperatureBatchToInsertMessage(
    TemperatureBatchItem[] TemperatureBatch
);

public sealed record TemperatureBatchItem
{
    public Guid Id { get; init; }
    
    public Guid PersonId { get; init; }
    
    public decimal Value { get; init; }
    
    public DateTime Timestamp { get; init; }
}