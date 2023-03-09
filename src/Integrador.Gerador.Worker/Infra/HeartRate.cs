namespace Integrador.Gerador.Worker.Infra;

public record HeartRate(
    Guid Id, Guid PersonId, int Reading /* TODO: Trocar para value */,
    DateTime Timestamp, bool ReadForIntegration
);
