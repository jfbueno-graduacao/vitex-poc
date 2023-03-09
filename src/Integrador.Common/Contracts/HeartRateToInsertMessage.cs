namespace Integrador.Common.Contracts;

public record HeartRateToInsertMessage
{
    public Guid Id { get; init; }
    public Guid PersonId { get; init; }
    public int Value { get; init; }
    public DateTime Timestamp { get; init; }
}