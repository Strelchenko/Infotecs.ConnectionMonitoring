namespace Core.Services;

/// <summary>
/// Interface for RabbitMq producer.
/// </summary>
public interface IRabbitMqProducer
{
    /// <summary>
    /// Send message about success adding new event.
    /// </summary>
    /// <param name="message">Message.</param>
    void SendSuccessEventMessage(string message);

    /// <summary>
    /// Send message about error adding new event.
    /// </summary>
    /// <param name="message">Message.</param>
    void SendErrorEventMessage(string message);
}
