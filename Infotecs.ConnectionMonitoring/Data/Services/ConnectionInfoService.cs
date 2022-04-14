using Core.Models;
using Core.Services;
using Data.Models;
using Data.Repositories;
using Data.UnitOfWork;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Data.Services;

/// <summary>
/// Service for work with ConnectionInfo.
/// </summary>
public class ConnectionInfoService : IConnectionInfoService
{
    private readonly ILogger<ConnectionInfoService> logger;
    private readonly IConnectionMonitoringRepository repository;
    private readonly IUnitOfWork unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionInfoService"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="repository">Repository.</param>
    /// <param name="unitOfWork">UnitOfWork.</param>
    public ConnectionInfoService(ILogger<ConnectionInfoService> logger, IConnectionMonitoringRepository repository, IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.repository = repository;
        this.unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Get all connections.
    /// </summary>
    /// <returns>List of connections.</returns>
    public async Task<ConnectionInfo[]> GetAllAsync()
    {
        logger.LogInformation("Get all devices");

        IEnumerable<ConnectionInfoEntity> result = await repository.GetAllConnectionsInfoAsync();

        return result.Adapt<ConnectionInfo[]>();
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

        ConnectionInfoEntity? exist = await repository.GetConnectionInfoByIdAsync(connectionInfo.Id);

        if (exist != null)
        {
            unitOfWork.BeginTransaction();
            try
            {
                await repository.UpdateConnectionInfoAsync(connectionInfo.Adapt<ConnectionInfoEntity>());
                logger.LogInformation($"Device with id {connectionInfo.Id} has been updated");
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                logger.LogError(e, "Error while updating ConnectionInfo");
                throw;
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }
        else
        {
            unitOfWork.BeginTransaction();
            try
            {
                await repository.CreateConnectionInfoAsync(connectionInfo.Adapt<ConnectionInfoEntity>());
                logger.LogInformation($"Device with id {connectionInfo.Id} has been added");
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                logger.LogError(e, "Error while creating ConnectionInfo");
                throw;
            }
            finally
            {
                unitOfWork.Dispose();
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

        ConnectionInfoEntity? exist = await repository.GetConnectionInfoByIdAsync(connectionInfo.Id);

        unitOfWork.BeginTransaction();
        try
        {
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

            foreach (ConnectionEvent connectionEvent in events)
            {
                logger.LogInformation("Event: {@ConnectionEvent}", connectionEvent);
                connectionEvent.Id ??= Guid.NewGuid().ToString();
                connectionEvent.ConnectionId = connectionInfo.Id;
                await repository.CreateConnectionEventAsync(connectionEvent.Adapt<ConnectionEventEntity>());
            }

            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            unitOfWork.Rollback();
            logger.LogError(e, $"Error while saving ConnectionInfo {connectionInfo.Id}");
            throw;
        }
        finally
        {
            unitOfWork.Dispose();
        }
    }
}
