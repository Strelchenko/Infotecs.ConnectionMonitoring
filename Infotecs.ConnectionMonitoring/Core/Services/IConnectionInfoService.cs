using Core.Models;

namespace Core.Services;

public interface IConnectionInfoService
{
    Task<ConnectionInfo[]> GetAllAsync();
    Task SaveAsync(ConnectionInfo connectionInfo);
}
