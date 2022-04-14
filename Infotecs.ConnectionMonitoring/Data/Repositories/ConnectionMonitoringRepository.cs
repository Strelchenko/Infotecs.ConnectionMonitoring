using Dapper;
using Data.Models;
using Microsoft.Extensions.Configuration;

namespace Data.Repositories;

/// <summary>
/// Repository for work with Db.
/// </summary>
public class ConnectionMonitoringRepository : IConnectionMonitoringRepository
{
    private readonly string connectionString;
    private DbSession session;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionMonitoringRepository"/> class.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    /// <param name="session">DbSession.</param>
    public ConnectionMonitoringRepository(IConfiguration configuration, DbSession session)
    {
        this.session = session;
        connectionString = configuration.GetConnectionString("InfotecsMonitoring");
    }

    /// <summary>
    /// Get connection by id.
    /// </summary>
    /// <param name="id">Connection identification.</param>
    /// <returns>Connection.</returns>
    public async Task<ConnectionInfoEntity?> GetConnectionInfoByIdAsync(string id)
    {
        var commandText = "SELECT * FROM \"ConnectionInfo\" WHERE \"Id\" = @id";

        var queryArgs = new { Id = id };

        return await session.Connection.QueryFirstOrDefaultAsync<ConnectionInfoEntity>(commandText, queryArgs, session.Transaction);
    }

    /// <summary>
    /// Get all connections.
    /// </summary>
    /// <returns>List of connections.</returns>
    public async Task<IEnumerable<ConnectionInfoEntity>> GetAllConnectionsInfoAsync()
    {
        var commandText = "SELECT * FROM \"ConnectionInfo\"";

        return await session.Connection.QueryAsync<ConnectionInfoEntity>(commandText, null, session.Transaction);
    }

    /// <summary>
    /// Create connection.
    /// </summary>
    /// <param name="connectionInfo">Connection.</param>
    /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
    public async Task CreateConnectionInfoAsync(ConnectionInfoEntity connectionInfo)
    {
        var commandText = "INSERT INTO \"ConnectionInfo\" (\"Id\", \"UserName\", \"Os\", \"AppVersion\", \"LastConnection\") VALUES (@Id, @UserName, @Os, @AppVersion, @LastConnection)";

        await session.Connection.ExecuteAsync(commandText, connectionInfo, session.Transaction);
    }

    /// <summary>
    /// Update connection.
    /// </summary>
    /// <param name="connectionInfo">Connection.</param>
    /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
    public async Task UpdateConnectionInfoAsync(ConnectionInfoEntity connectionInfo)
    {
        var commandText = "UPDATE \"ConnectionInfo\" SET \"UserName\" = @UserName, \"Os\" = @Os, \"AppVersion\" = @AppVersion, \"LastConnection\" = @LastConnection WHERE \"Id\" = @id";

        await session.Connection.ExecuteAsync(commandText, connectionInfo, session.Transaction);
    }

    /// <summary>
    /// Delete connection.
    /// </summary>
    /// <param name="id">Connection identification.</param>
    /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
    public async Task DeleteConnectionInfoAsync(string id)
    {
        var commandText = "DELETE FROM \"ConnectionInfo\" WHERE \"Id\" = @id";

        var queryArguments = new { Id = id };

        await session.Connection.ExecuteAsync(commandText, queryArguments, session.Transaction);
    }

    /// <summary>
    /// Get connection event by id.
    /// </summary>
    /// <param name="id">ConnectionEvent identification.</param>
    /// <returns>ConnectionEvent.</returns>
    public async Task<ConnectionEventEntity?> GetConnectionEventByIdAsync(string id)
    {
        var commandText = "SELECT * FROM \"ConnectionEvent\" WHERE \"Id\" = @id";

        var queryArgs = new { Id = id };

        return await session.Connection.QueryFirstOrDefaultAsync<ConnectionEventEntity>(commandText, queryArgs, session.Transaction);
    }

    /// <summary>
    /// Get all events for current connection.
    /// </summary>
    /// <param name="connectionId">Connection Id.</param>
    /// <returns>List of events.</returns>
    public async Task<IEnumerable<ConnectionEventEntity>> GetEventsByConnectionIdAsync(string connectionId)
    {
        var commandText = "SELECT * FROM \"ConnectionEvent\" WHERE \"ConnectionId\" = @connectionId";

        var queryArgs = new { ConnectionId = connectionId };

        return await session.Connection.QueryAsync<ConnectionEventEntity>(commandText, queryArgs, session.Transaction);
    }

    /// <summary>
    /// Create event.
    /// </summary>
    /// <param name="connectionEvent">Event.</param>
    /// <returns>Task.</returns>
    public async Task CreateConnectionEventAsync(ConnectionEventEntity connectionEvent)
    {
        var commandText = "INSERT INTO \"ConnectionEvent\" (\"Id\", \"Name\", \"ConnectionId\", \"EventTime\") VALUES (@Id, @Name, @ConnectionId, @EventTime)";

        await session.Connection.ExecuteAsync(commandText, connectionEvent, session.Transaction);
    }
}
