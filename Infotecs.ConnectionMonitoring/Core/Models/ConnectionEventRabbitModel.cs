namespace Core.Models;

/// <summary>
/// Connection event model for Rabbit message.
/// </summary>
public class ConnectionEventRabbitModel
{
    /// <summary>
    /// Event Id.
    /// </summary>
    public string? EventId { get; set; }

    /// <summary>
    /// Name of event.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Connection Id.
    /// </summary>
    public string? NodeId { get; set; }

    /// <summary>
    /// Time of the event.
    /// </summary>
    public DateTime Date { get; set; }
}
