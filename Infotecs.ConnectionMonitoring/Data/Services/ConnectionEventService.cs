using Core.Models;
using Core.Services;
using Data.Models;
using Data.UnitOfWork;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Data.Services;

/// <summary>
/// Service for work with ConnectionEvent.
/// </summary>
public class ConnectionEventService : IConnectionEventService
{
    private readonly ILogger<ConnectionInfoService> logger;
    private readonly IConfiguration configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionEventService"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="configuration">IConfiguration.</param>
    public ConnectionEventService(ILogger<ConnectionInfoService> logger, IConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
    }

    /// <summary>
    /// Get all events for current connection.
    /// </summary>
    /// <param name="connectionId">ConnectionId.</param>
    /// <returns>List of events.</returns>
    public async Task<ConnectionEvent[]> GetEventsByConnectionIdAsync(string connectionId)
    {
        logger.LogInformation($"Get all events by connection id - {connectionId}");

        using (var unitOfWork = new DapperUnitOfWork(configuration))
        {
            var result = await unitOfWork.ConnectionMonitoringRepository.GetEventsByConnectionIdAsync(connectionId);

            return result.Adapt<ConnectionEvent[]>();
        }
    }

    /// <summary>
    /// Create events.
    /// </summary>
    /// <param name="connectionEvents">Events list.</param>
    /// <returns>Task.</returns>
    public async Task SaveEventsAsync(IEnumerable<ConnectionEvent> connectionEvents)
    {
        using (var unitOfWork = new DapperUnitOfWork(configuration))
        {
            foreach (ConnectionEvent connectionEvent in connectionEvents)
            {
                try
                {
                    logger.LogInformation("Event: {@ConnectionEvent}", connectionEvent);
                    connectionEvent.Id ??= Guid.NewGuid().ToString();
                    await unitOfWork.ConnectionMonitoringRepository.CreateConnectionEventAsync(connectionEvent.Adapt<ConnectionEventEntity>());
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Event saving error {@ConnectionEvent}", connectionEvent);
                    throw;
                }
            }
        }
    }
}
