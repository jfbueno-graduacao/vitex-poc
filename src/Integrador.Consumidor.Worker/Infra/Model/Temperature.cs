using System;

namespace Integrador.Consumidor.Worker.Infra.Model;

internal record Temperature(Guid Id, Guid PersonId, decimal Value, DateTime Timestamp);
