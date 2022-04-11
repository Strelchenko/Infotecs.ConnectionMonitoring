using Dapper;
using Data.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Data.Repositories;

public class ConnectionMonitoringRepository : IConnectionMonitoringRepository
{
    private readonly string connectionString;

    public ConnectionMonitoringRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("InfotecsMonitoring");
    }

    public async Task<ConnectionInfoEntity?> GetConnectionInfoByIdAsync(string id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "SELECT * FROM \"ConnectionInfo\" WHERE \"Id\" = @id";

            var queryArgs = new { Id = id };

            return await connection.QueryFirstOrDefaultAsync<ConnectionInfoEntity>(commandText, queryArgs);
        }
    }

    public async Task<IEnumerable<ConnectionInfoEntity>> GetAllConnectionsInfoAsync()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "SELECT * FROM \"ConnectionInfo\"";

            return await connection.QueryAsync<ConnectionInfoEntity>(commandText);
        }
    }

    public async Task CreateConnectionInfoAsync(ConnectionInfoEntity connectionInfo)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "INSERT INTO \"ConnectionInfo\" (\"Id\", \"UserName\", \"Os\", \"AppVersion\", \"LastConnection\") VALUES (@Id, @UserName, @Os, @AppVersion, @LastConnection)";

            var queryArguments = new
            {
                id = connectionInfo.Id,
                userName = connectionInfo.UserName,
                os = connectionInfo.Os,
                appVersion = connectionInfo.AppVersion,
                lastConnection = connectionInfo.LastConnection,
            };

            await connection.ExecuteAsync(commandText, queryArguments);
        }
    }

    public async Task UpdateConnectionInfoAsync(ConnectionInfoEntity connectionInfo)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "UPDATE \"ConnectionInfo\" SET \"UserName\" = @UserName, \"Os\" = @Os, \"AppVersion\" = @AppVersion, \"LastConnection\" = @LastConnection WHERE \"Id\" = @id";

            var queryArgs = new
            {
                id = connectionInfo.Id,
                userName = connectionInfo.UserName,
                os = connectionInfo.Os,
                appVersion = connectionInfo.AppVersion,
                lastConnection = connectionInfo.LastConnection,
            };

            await connection.ExecuteAsync(commandText, queryArgs);
        }
    }

    public async Task DeleteConnectionInfoAsync(string id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "DELETE FROM \"ConnectionInfo\" WHERE \"Id\" = @id";

            var queryArguments = new { Id = id };

            await connection.ExecuteAsync(commandText, queryArguments);
        }
    }
}
