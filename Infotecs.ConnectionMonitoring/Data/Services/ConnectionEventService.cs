using Core.Models;
using Core.Services;
using Data.Models;
using Data.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Data.Services;

/// <summary>
/// Service for work with ConnectionEvent.
/// </summary>
public class ConnectionEventService : IConnectionEventService
{
    private readonly ILogger<ConnectionInfoService> logger;
    private readonly IConnectionMonitoringRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionEventService"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="repository">Repository.</param>
    public ConnectionEventService(ILogger<ConnectionInfoService> logger, IConnectionMonitoringRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    /// <summary>
    /// Get all events for current connection.
    /// </summary>
    /// <param name="connectionId">ConnectionId.</param>
    /// <returns>List of events.</returns>
    public async Task<ConnectionEvent[]> GetEventsByConnectionIdAsync(string connectionId)
    {
        logger.LogInformation($"Get all events by connection id - {connectionId}");

        var result = await repository.GetEventsByConnectionIdAsync(connectionId);

        return result.Adapt<ConnectionEvent[]>();
    }

    /// <summary>
    /// Create events.
    /// </summary>
    /// <param name="connectionEvents">Events list.</param>
    /// <returns>Task.</returns>
    public async Task SaveEventsAsync(IEnumerable<ConnectionEvent> connectionEvents)
    {
        foreach (ConnectionEvent connectionEvent in connectionEvents)
        {
            try
            {
                logger.LogInformation("Event: {@ConnectionEvent}", connectionEvent);
                connectionEvent.Id ??= Guid.NewGuid().ToString();
                await repository.CreateConnectionEventAsync(connectionEvent.Adapt<ConnectionEventEntity>());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Event saving error {@ConnectionEvent}", connectionEvent);
                throw;
            }
        }
    }
}
