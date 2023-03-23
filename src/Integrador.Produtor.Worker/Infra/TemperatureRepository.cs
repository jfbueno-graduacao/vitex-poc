using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Integrador.Produtor.Worker.Infra;

internal sealed class TemperatureRepository : IDisposable
{
    private readonly IDbConnection _connection;

    public TemperatureRepository()
    {
        _connection = new SqlConnection(
            "Server=localhost,54331;Database=VitalSignReadings;User Id=sa;Password=4eFmpjPQLJK2NsoPrBQugHjo;Encrypt=False"
        );

        _connection.Open();
    }

    public async Task<IReadOnlyCollection<Temperature>> GetNotIntegratedValues()
    {
        // TODO: Remover '*'
        IEnumerable<Temperature>? data = await _connection.QueryAsync<Temperature>(
            @"  DROP TABLE IF EXISTS #Values;
            
                SELECT TOP 75 * INTO #Values
                FROM [Temperature] 
                WHERE ReadForIntegration = 0 ORDER BY [Timestamp] ASC;

                UPDATE [Temperature]
                SET ReadForIntegration = 1
                WHERE Id IN (SELECT Id FROM #Values);

                SELECT * FROM #Values;"
        );

        return data?.ToArray() ?? Array.Empty<Temperature>();
    }

    public async Task<Temperature?> GetFirstNotIntegratedValue()
    {
        var data = await _connection.QueryFirstOrDefaultAsync<Temperature>(
            "SELECT TOP 1 * FROM [Temperature] " +
            "WHERE ReadForIntegration = 0 " +
            "ORDER BY [Timestamp] ASC"
        );

        return data;
    }
    
    public void Dispose()
    {
        _connection.Dispose();
    }
}
