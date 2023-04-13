using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Connector.Producer.Worker.Infra.Database;

internal sealed class TemperatureRepository : IDisposable
{
    private readonly IDbConnection _connection;

    public TemperatureRepository(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Default connection string is empty");

        _connection = new SqlConnection(connectionString);

        _connection.Open();
    }

    /// <summary>
    /// Lista registros de temperatura que não
    /// foram integrados e seta a flag de integração
    /// </summary>
    /// <returns>Lista de registro de temperaturas para serem integradas</returns>
    public async Task<IReadOnlyCollection<Temperature>> GetNotIntegratedValues()
    {
        IEnumerable<Temperature>? data = await _connection.QueryAsync<Temperature>(
            @"
                DROP TABLE IF EXISTS #Values;
            
                SELECT TOP 500 * INTO #Values
                FROM [Temperature] 
                WHERE ReadForIntegration = 0 ORDER BY [Timestamp] ASC;

                UPDATE [Temperature]
                SET ReadForIntegration = 1
                WHERE Id IN (SELECT Id FROM #Values);

                SELECT * FROM #Values;"
        );

        return data?.ToArray() ?? Array.Empty<Temperature>();
    }

    /// <summary>
    /// Lista as pessoas com temperatura mais alta do que o threshold 
    /// </summary>
    /// <returns>Coleção com os ids das pessoas com temperatura alta</returns>
    public async Task<IReadOnlyCollection<Guid>> ListPeopleWithHighTemperature()
    {
        var peopleIds = await _connection.QueryAsync<Guid>(
            @"
                SELECT 
	                PersonId
                FROM [Temperature]
                WHERE [Value] >= 38
                AND [ReadForIntegration] = 0
                GROUP BY PersonId"
        );

        return peopleIds?.ToArray() ?? Array.Empty<Guid>();
    }

    /// <summary>
    /// Lista registros de temperatura que não foram integrados
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyCollection<Temperature>> GetNotIntegratedValuesFromPeopleIds(
        IReadOnlyCollection<Guid> peopleIds
    )
    {
        IEnumerable<Temperature> data = await _connection.QueryAsync<Temperature>(
            @"
                DROP TABLE IF EXISTS #HighTempValues;
            
                SELECT TOP 75 * INTO #HighTempValues
                FROM [Temperature] 
                WHERE ReadForIntegration = 0 
                AND PersonId IN @ids
                ORDER BY [Timestamp] ASC;

                UPDATE [Temperature]
                SET ReadForIntegration = 1
                WHERE Id IN (SELECT Id FROM #HighTempValues);

                SELECT * FROM #HighTempValues;",
            new { ids = peopleIds }
        );

        return data?.ToArray() ?? Array.Empty<Temperature>();
    }

    public void Dispose()
        => _connection.Dispose();
}
