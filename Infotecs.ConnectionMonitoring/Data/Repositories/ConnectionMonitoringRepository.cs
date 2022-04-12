using Core.Models;
using Dapper;
using Data.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Data.Repositories;

/// <summary>
/// Repository for work with Db.
/// </summary>
public class ConnectionMonitoringRepository : IConnectionMonitoringRepository
{
    private readonly string connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionMonitoringRepository"/> class.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    public ConnectionMonitoringRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("InfotecsMonitoring");
    }

    /// <summary>
    /// Get connection by id.
    /// </summary>
    /// <param name="id">Connection identification.</param>
    /// <returns>Connection.</returns>
    public async Task<ConnectionInfoEntity?> GetConnectionInfoByIdAsync(string id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "SELECT * FROM \"ConnectionInfo\" WHERE \"Id\" = @id";

            var queryArgs = new { Id = id };

            return await connection.QueryFirstOrDefaultAsync<ConnectionInfoEntity>(commandText, queryArgs);
        }
    }

    /// <summary>
    /// Get all connections.
    /// </summary>
    /// <returns>List of connections.</returns>
    public async Task<IEnumerable<ConnectionInfoEntity>> GetAllConnectionsInfoAsync()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "SELECT * FROM \"ConnectionInfo\"";

            return await connection.QueryAsync<ConnectionInfoEntity>(commandText);
        }
    }

    /// <summary>
    /// Create connection.
    /// </summary>
    /// <param name="connectionInfo">Connection.</param>
    /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
    public async Task CreateConnectionInfoAsync(ConnectionInfoEntity connectionInfo)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "INSERT INTO \"ConnectionInfo\" (\"Id\", \"UserName\", \"Os\", \"AppVersion\", \"LastConnection\") VALUES (@Id, @UserName, @Os, @AppVersion, @LastConnection)";

            await connection.ExecuteAsync(commandText, connectionInfo);
        }
    }

    /// <summary>
    /// Update connection.
    /// </summary>
    /// <param name="connectionInfo">Connection.</param>
    /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
    public async Task UpdateConnectionInfoAsync(ConnectionInfoEntity connectionInfo)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "UPDATE \"ConnectionInfo\" SET \"UserName\" = @UserName, \"Os\" = @Os, \"AppVersion\" = @AppVersion, \"LastConnection\" = @LastConnection WHERE \"Id\" = @id";

            await connection.ExecuteAsync(commandText, connectionInfo);
        }
    }

    /// <summary>
    /// Delete connection.
    /// </summary>
    /// <param name="id">Connection identification.</param>
    /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
    public async Task DeleteConnectionInfoAsync(string id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "DELETE FROM \"ConnectionInfo\" WHERE \"Id\" = @id";

            var queryArguments = new { Id = id };

            await connection.ExecuteAsync(commandText, queryArguments);
        }
    }

    /// <summary>
    /// Get all events for current connection.
    /// </summary>
    /// <param name="connectionId">Connection Id.</param>
    /// <returns>List of events.</returns>
    public async Task<IEnumerable<ConnectionEventEntity>> GetEventsByConnectionIdAsync(string connectionId)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "SELECT * FROM \"ConnectionEvent\" WHERE \"ConnectionId\" = @connectionId";

            var queryArgs = new { ConnectionId = connectionId };

            return await connection.QueryAsync<ConnectionEventEntity>(commandText, queryArgs);
        }
    }

    /// <summary>
    /// Create event.
    /// </summary>
    /// <param name="connectionEvent">Event.</param>
    /// <returns>Task.</returns>
    public async Task CreateConnectionEventAsync(ConnectionEventEntity connectionEvent)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            var commandText = "INSERT INTO \"ConnectionEvent\" (\"Id\", \"Name\", \"ConnectionId\", \"EventTime\") VALUES (@Id, @Name, @ConnectionId, @EventTime)";

            await connection.ExecuteAsync(commandText, connectionEvent);
        }
    }
}
