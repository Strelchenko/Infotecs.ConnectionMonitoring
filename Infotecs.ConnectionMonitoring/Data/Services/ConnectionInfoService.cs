using Core.Models;
using Core.Services;
using Data.Models;
using Data.UnitOfWork;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Data.Services;

/// <summary>
/// Service for work with ConnectionInfo.
/// </summary>
public class ConnectionInfoService : IConnectionInfoService
{
    private readonly ILogger<ConnectionInfoService> logger;
    private readonly IConfiguration configuration;
    private readonly IHubContext<ConnectionInfoHub> hubContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionInfoService"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="configuration">IConfiguration.</param>
    /// <param name="hubContext">ConnectionInfoHub.</param>
    public ConnectionInfoService(ILogger<ConnectionInfoService> logger, IConfiguration configuration, IHubContext<ConnectionInfoHub> hubContext)
    {
        this.logger = logger;
        this.configuration = configuration;
        this.hubContext = hubContext;
    }

    /// <summary>
    /// Get all connections.
    /// </summary>
    /// <returns>List of connections.</returns>
    public async Task<ConnectionInfo[]> GetAllAsync()
    {
        logger.LogInformation("Get all devices");

        using (var unitOfWork = new DapperUnitOfWork(configuration))
        {
            IEnumerable<ConnectionInfoEntity> result = await unitOfWork.ConnectionMonitoringRepository.GetAllConnectionsInfoAsync();

            return result.Adapt<ConnectionInfo[]>();
        }
    }

    /// <summary>
    /// Create or update connection info.
    /// </summary>
    /// <param name="connectionInfo">Connection info.</param>
    /// <returns>Task.</returns>
    /// <exception cref="Exception">Exception.</exception>
    public async Task SaveAsync(ConnectionInfo connectionInfo)
    {
        if (connectionInfo.Id == null)
        {
            logger.LogError("Id can not be null");
            throw new Exception("Id can not be null");
        }

        using (var unitOfWork = new DapperUnitOfWork(configuration))
        {
            ConnectionInfoEntity? exist = await unitOfWork.ConnectionMonitoringRepository.GetConnectionInfoByIdAsync(connectionInfo.Id);

            try
            {
                if (exist != null)
                {
                    await unitOfWork.ConnectionMonitoringRepository.UpdateConnectionInfoAsync(connectionInfo.Adapt<ConnectionInfoEntity>());
                    logger.LogInformation($"Device with id {connectionInfo.Id} has been updated");
                }
                else
                {
                    await unitOfWork.ConnectionMonitoringRepository.CreateConnectionInfoAsync(connectionInfo.Adapt<ConnectionInfoEntity>());
                    logger.LogInformation($"Device with id {connectionInfo.Id} has been added");
                }

                unitOfWork.Commit();

                if (exist == null)
                {
                    await hubContext.Clients.All.SendAsync("onNewConnectionInfoAdded", connectionInfo);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error while saving ConnectionInfo {connectionInfo.Id}");
                throw;
            }
        }
    }

    /// <summary>
    /// Create or update connection info with events.
    /// </summary>
    /// <param name="connectionInfo">Connection info.</param>
    /// <param name="events">List of connection info events.</param>
    /// <returns>Task.</returns>
    /// <exception cref="Exception">Exception.</exception>
    public async Task SaveAsync(ConnectionInfo connectionInfo, IEnumerable<ConnectionEvent> events)
    {
        if (connectionInfo.Id == null)
        {
            logger.LogError("Id can not be null");
            throw new Exception("Id can not be null");
        }

        using (var unitOfWork = new DapperUnitOfWork(configuration))
        {
            ConnectionInfoEntity? exist = await unitOfWork.ConnectionMonitoringRepository.GetConnectionInfoByIdAsync(connectionInfo.Id);
            
            try
            {
                if (exist != null)
                {
                    await unitOfWork.ConnectionMonitoringRepository.UpdateConnectionInfoAsync(connectionInfo.Adapt<ConnectionInfoEntity>());
                    logger.LogInformation($"Device with id {connectionInfo.Id} has been updated");
                }
                else
                {
                    await unitOfWork.ConnectionMonitoringRepository.CreateConnectionInfoAsync(connectionInfo.Adapt<ConnectionInfoEntity>());
                    logger.LogInformation($"Device with id {connectionInfo.Id} has been added");
                }

                foreach (ConnectionEvent connectionEvent in events)
                {
                    logger.LogInformation("Event: {@ConnectionEvent}", connectionEvent);
                    connectionEvent.Id ??= Guid.NewGuid().ToString();
                    connectionEvent.ConnectionId = connectionInfo.Id;
                    await unitOfWork.ConnectionMonitoringRepository.CreateConnectionEventAsync(connectionEvent.Adapt<ConnectionEventEntity>());
                }

                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error while saving ConnectionInfo {connectionInfo.Id}");
                throw;
            }
        }
    }
}
