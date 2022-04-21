using System.Text.Json;
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
    private readonly IRabbitMqProducer rabbitMqProducer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionEventService"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="configuration">IConfiguration.</param>
    /// <param name="rabbitMqProducer">IRabbitMqProducer.</param>
    public ConnectionEventService(ILogger<ConnectionInfoService> logger, IConfiguration configuration, IRabbitMqProducer rabbitMqProducer)
    {
        this.logger = logger;
        this.configuration = configuration;
        this.rabbitMqProducer = rabbitMqProducer;
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
                var eventMessage = new ConnectionEventRabbitModel();

                try
                {
                    eventMessage = connectionEvent.Adapt<ConnectionEventRabbitModel>(TypeAdapterConfig<ConnectionEvent, ConnectionEventRabbitModel>
                        .NewConfig()
                        .Map(dest => dest.EventId, src => src.Id)
                        .Map(dest => dest.NodeId, src => src.ConnectionId)
                        .Map(dest => dest.Name, src => src.Name)
                        .Map(dest => dest.Date, src => src.EventTime)
                        .Config);

                    logger.LogInformation("Event: {@ConnectionEvent}", connectionEvent);
                    connectionEvent.Id ??= Guid.NewGuid().ToString();
                    await unitOfWork.ConnectionMonitoringRepository.CreateConnectionEventAsync(connectionEvent.Adapt<ConnectionEventEntity>());
                    unitOfWork.Commit();

                    rabbitMqProducer.SendSuccessEventMessage(JsonSerializer.Serialize(eventMessage));
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Event saving error {@ConnectionEvent}", connectionEvent);
                    rabbitMqProducer.SendErrorEventMessage(JsonSerializer.Serialize(eventMessage));
                }
            }
        }
    }
}
