using Dapper;
using Integrador.Faker.Worker.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Integrador.Faker.Worker.Repositories;

public sealed class PeopleRepository : IDisposable
{
    private readonly IDbConnection _connection;

    public PeopleRepository(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Default connection string is empty");

        _connection = new SqlConnection(connectionString);

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