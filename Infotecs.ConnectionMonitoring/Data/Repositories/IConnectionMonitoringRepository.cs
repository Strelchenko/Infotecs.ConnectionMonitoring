using Data.Models;

namespace Data.Repositories;

public interface IConnectionMonitoringRepository
{
    /// <summary>
    /// Get connection by id
    /// </summary>
    /// <param name="id">Connection identification</param>
    /// <returns></returns>
    Task<ConnectionInfoEntity?> GetConnectionInfoByIdAsync(string id);

    /// <summary>
    /// Get all connections
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ConnectionInfoEntity>> GetAllConnectionsInfoAsync();

    /// <summary>
    /// Create connection
    /// </summary>
    /// <param name="connectionInfo">Connection</param>
    /// <returns></returns>
    Task CreateConnectionInfoAsync(ConnectionInfoEntity connectionInfo);

    /// <summary>
    /// Update connection
    /// </summary>
    /// <param name="connectionInfo">Connection</param>
    /// <returns></returns>
    Task UpdateConnectionInfoAsync(ConnectionInfoEntity connectionInfo);

    /// <summary>
    /// Delete connection
    /// </summary>
    /// <param name="id">Connection identification</param>
    /// <returns></returns>
    Task DeleteConnectionInfoAsync(string id);
}
