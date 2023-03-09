using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Integrador.Gerador.Worker.Infra;

internal sealed class HeartRateRepository : IDisposable
{
    private readonly IDbConnection _connection;

    public HeartRateRepository()
    {
        _connection = new SqlConnection(
            "Server=localhost,54331;Database=VitalSigns;User Id=sa;Password=4eFmpjPQLJK2NsoPrBQugHjo;Encrypt=False"
        );

        _connection.Open();
    }

    public async Task<IReadOnlyCollection<HeartRate>> GetNotIntegratedValues()
    {
        IEnumerable<HeartRate>? data = await _connection.QueryAsync<HeartRate>(
            "SELECT TOP 75 * FROM [HeartRate] WHERE ReadForIntegration = 0 ORDER BY [Timestamp] ASC"
        );

        return data?.ToArray() ?? Array.Empty<HeartRate>();
    }

    public async Task<HeartRate?> GetFirstNotIntegratedValue()
    {
        var data = await _connection.QueryFirstOrDefaultAsync<HeartRate>(
            "SELECT TOP 1 * FROM [HeartRate] " +
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
