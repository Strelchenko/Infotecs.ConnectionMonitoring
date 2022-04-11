using Core.Models;
using Core.Services;
using Data.Models;
using Data.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Data.Services;

public class ConnectionInfoService : IConnectionInfoService
{
    private readonly ILogger<ConnectionInfoService> logger;
    private readonly IConnectionMonitoringRepository repository;

    public ConnectionInfoService(ILogger<ConnectionInfoService> logger, IConnectionMonitoringRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    /// <summary>
    /// Get all connections
    /// </summary>
    /// <returns></returns>
    public async Task<ConnectionInfo[]> GetAllAsync()
    {
        logger.LogInformation("Get all devices");

        IEnumerable<ConnectionInfoEntity> result = await repository.GetAllConnectionsInfoAsync();

        return result.Adapt<ConnectionInfo[]>();
    }

    /// <summary>
    /// Create or update connection info
    /// </summary>
    /// <param name="connectionInfo">Connection info</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task SaveAsync(ConnectionInfo connectionInfo)
    {
        if (connectionInfo.Id == null)
        {
            logger.LogError("Id can not be null");
            throw new Exception("Id can not be null");
        }

        ConnectionInfoEntity? exist = await repository.GetConnectionInfoByIdAsync(connectionInfo.Id);

        if (exist != null)
        {
            await repository.UpdateConnectionInfoAsync(connectionInfo.Adapt<ConnectionInfoEntity>());
            logger.LogInformation($"Device with id {connectionInfo.Id} has been updated");
        }
        else
        {
            await repository.CreateConnectionInfoAsync(connectionInfo.Adapt<ConnectionInfoEntity>());
            logger.LogInformation($"Device with id {connectionInfo.Id} has been added");
        }
    }
}
