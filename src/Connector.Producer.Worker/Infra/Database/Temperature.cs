namespace Connector.Producer.Worker.Infra.Database;

public sealed record Temperature(
    Guid Id, Guid PersonId, decimal Value, DateTime Timestamp
);
