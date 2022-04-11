using Data.Models;

namespace Data.Repositories;

public interface IConnectionMonitoringRepository
{
    Task<ConnectionInfoEntity?> GetConnectionInfoByIdAsync(string id);
    Task<IEnumerable<ConnectionInfoEntity>> GetAllConnectionsInfoAsync();
    Task CreateConnectionInfoAsync(ConnectionInfoEntity connectionInfo);
    Task UpdateConnectionInfoAsync(ConnectionInfoEntity connectionInfo);
    Task DeleteConnectionInfoAsync(string id);
}
