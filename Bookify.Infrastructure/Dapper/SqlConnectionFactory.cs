using System.Data;
using Bookify.Application.Abstractions.Data;
using Npgsql;

namespace Bookify.Infrastructure.Dapper;

internal sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();

        return dbConnection;
    }
}