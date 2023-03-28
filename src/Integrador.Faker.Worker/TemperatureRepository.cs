using Dapper;
using Integrador.Faker.Worker.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Integrador.Faker.Worker;

public sealed class TemperatureRepository : IDisposable
{
    private readonly IDbConnection _connection;

    public TemperatureRepository(IConfiguration configuration, ILogger<TemperatureRepository> logger)
    {
        var cs = configuration.GetConnectionString("Default");

        _connection = new SqlConnection(cs);
        
        logger.LogInformation("Conectando ao banco: {ConnStr}", cs);

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