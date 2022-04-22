using Core.Models;

namespace Core.Services;

/// <summary>
/// Interface for RabbitMq producer.
/// </summary>
public interface IRabbitMqProducer
{
    /// <summary>
    /// Send message about success adding new event.
    /// </summary>
    /// <param name="connectionEvent">ConnectionEvent.</param>
    void SendSuccessEventMessage(ConnectionEvent connectionEvent);

    /// <summary>
    /// Send message about error adding new event.
    /// </summary>
    /// <param name="connectionEvent">ConnectionEvent.</param>
    void SendErrorEventMessage(ConnectionEvent connectionEvent);
}
