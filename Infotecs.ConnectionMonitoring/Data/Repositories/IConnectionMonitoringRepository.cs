using Data.Models;

namespace Data.Repositories;

/// <summary>
/// Interface for ConnectionMonitoringRepository.
/// </summary>
public interface IConnectionMonitoringRepository
{
    /// <summary>
    /// Get connection by id.
    /// </summary>
    /// <param name="id">Connection identification.</param>
    /// <returns>Connection.</returns>
    Task<ConnectionInfoEntity?> GetConnectionInfoByIdAsync(string id);

    /// <summary>
    /// Get all connections.
    /// </summary>
    /// <returns>List of connections.</returns>
    Task<IEnumerable<ConnectionInfoEntity>> GetAllConnectionsInfoAsync();

    /// <summary>
    /// Create connection.
    /// </summary>
    /// <param name="connectionInfo">Connection.</param>
    /// <returns>Task.</returns>
    Task CreateConnectionInfoAsync(ConnectionInfoEntity connectionInfo);

    /// <summary>
    /// Update connection.
    /// </summary>
    /// <param name="connectionInfo">Connection.</param>
    /// <returns>Task.</returns>
    Task UpdateConnectionInfoAsync(ConnectionInfoEntity connectionInfo);

    /// <summary>
    /// Delete connection.
    /// </summary>
    /// <param name="id">Connection identification.</param>
    /// <returns>Task.</returns>
    Task DeleteConnectionInfoAsync(string id);

    /// <summary>
    /// Get connection event by id.
    /// </summary>
    /// <param name="id">ConnectionEvent identification.</param>
    /// <returns>ConnectionEvent.</returns>
    Task<ConnectionEventEntity?> GetConnectionEventByIdAsync(string id);

    /// <summary>
    /// Get all events for current connection.
    /// </summary>
    /// <param name="connectionId">Connection Id.</param>
    /// <returns>List of events.</returns>
    Task<IEnumerable<ConnectionEventEntity>> GetEventsByConnectionIdAsync(string connectionId);

    /// <summary>
    /// Create event.
    /// </summary>
    /// <param name="connectionEvent">Event.</param>
    /// <returns>Task.</returns>
    Task CreateConnectionEventAsync(ConnectionEventEntity connectionEvent);
}
