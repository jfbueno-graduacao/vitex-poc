namespace Connector.Faker.Worker.Model;

public sealed record Temperature(Guid PersonId, decimal Value, DateTime Timestamp);