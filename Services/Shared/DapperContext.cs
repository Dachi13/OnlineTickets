using Npgsql;
using Microsoft.Extensions.Configuration;

namespace Shared;

public class DapperContext(IConfiguration configuration) : IDisposable
{
    private readonly string _connectionString = configuration.GetConnectionString("Database")!;
    private NpgsqlConnection? _connection;

    public async Task<NpgsqlConnection> CreateConnectionAsync()
    {
        _connection = new NpgsqlConnection(_connectionString);
        await _connection.OpenAsync();

        return _connection;
    }

    public void Dispose()
    {
        _connection?.DisposeAsync();
    }
}