using Dapper;
using Integrador.Faker.Worker.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Integrador.Faker.Worker;

public sealed class TemperatureRepository : IDisposable
{
    private readonly IDbConnection _connection;

    public TemperatureRepository()
    {
        _connection = new SqlConnection(
            "Server=localhost,54331;Database=VitalSignReadings;User Id=sa;Password=4eFmpjPQLJK2NsoPrBQugHjo;Encrypt=False"
        );

        _connection.Open();
    }

    public async Task Insert(Temperature temperatureToInsert)
    {
        await _connection.ExecuteAsync(
            @"INSERT INTO [Temperature] (PersonId, Value, Timestamp)
                  VALUES (@personId, @value, @timestamp)",
            new
            {
                personId = temperatureToInsert.PersonId,
                value = temperatureToInsert.Value,
                timestamp = temperatureToInsert.Timestamp
            }
        );
    }

    public async Task<int> Insert(IEnumerable<Temperature> temperatureReadingsToInsert)
    {
        return await _connection.ExecuteAsync(
            "INSERT INTO [Temperature] (PersonId, [Value], [Timestamp])" +
            " VALUES (@PersonId, @Value, @Timestamp)",
            temperatureReadingsToInsert
        );
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}