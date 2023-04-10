using System;

namespace Connector.Consumer.Worker.Infra.Model;

internal record Temperature(Guid Id, Guid PersonId, decimal Value, DateTime Timestamp);
