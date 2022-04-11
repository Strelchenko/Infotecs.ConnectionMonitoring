using Core.Models;

namespace Core.Services;

public interface IConnectionInfoService
{
    /// <summary>
    /// Get all connections
    /// </summary>
    /// <returns></returns>
    Task<ConnectionInfo[]> GetAllAsync();

    /// <summary>
    /// Create or update connection info
    /// </summary>
    /// <param name="connectionInfo">Connection info</param>
    /// <returns></returns>
    Task SaveAsync(ConnectionInfo connectionInfo);
}
