namespace Integrador.Produtor.Worker.Infra.Database;

public record Temperature(
    Guid Id, Guid PersonId, decimal Value,
    DateTime Timestamp, bool ReadForIntegration
);
