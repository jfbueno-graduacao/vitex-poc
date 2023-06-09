namespace Connector.Faker.Worker.Model;

public sealed record Person(Guid Id, DateTime BirthDate, decimal BaseTemperature)
{
    private static readonly Random Random = new();

    public decimal VariateTemperature()
    {
        var variation = (decimal)Random.Next(0, 2) / 10;
        return BaseTemperature + variation;
    }
}