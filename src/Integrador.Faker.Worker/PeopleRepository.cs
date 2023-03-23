using Dapper;
using Integrador.Faker.Worker.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Integrador.Faker.Worker;

public sealed class PeopleRepository : IDisposable
{
    private readonly IDbConnection _connection;

    public PeopleRepository()
    {
        _connection = new SqlConnection(
            "Server=localhost,54331;Database=VitalSignReadings;User Id=sa;Password=4eFmpjPQLJK2NsoPrBQugHjo;Encrypt=False"
        );

        _connection.Open();
    }

    public async Task<IReadOnlyCollection<Person>> GetAll()
    {
        return (await _connection.QueryAsync<Person>(
            @"SELECT Id, BirthDate, BaseTemperature FROM [People]"
        )).ToArray();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}