namespace Integrador.Produtor.Worker.Infra;

public record Temperature(
    Guid Id, Guid PersonId, decimal Value,
    DateTime Timestamp, bool ReadForIntegration
);
