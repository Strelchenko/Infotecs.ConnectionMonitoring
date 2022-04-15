using Core.Models;

namespace Core.Services;

/// <summary>
/// ConnectionInfoService interface.
/// </summary>
public interface IConnectionInfoService
{
    /// <summary>
    /// Get all connections.
    /// </summary>
    /// <returns>List of connections.</returns>
    Task<ConnectionInfo[]> GetAllAsync();

    /// <summary>
    /// Create or update connection info.
    /// </summary>
    /// <param name="connectionInfo">Connection info.</param>
    /// <returns>Task.</returns>
    Task SaveAsync(ConnectionInfo connectionInfo);

    /// <summary>
    /// Create or update connection info with events.
    /// </summary>
    /// <param name="connectionInfo">Connection info.</param>
    /// <param name="events">List of connection info events.</param>
    /// <returns>Task.</returns>
    Task SaveAsync(ConnectionInfo connectionInfo, IEnumerable<ConnectionEvent> events);
}
