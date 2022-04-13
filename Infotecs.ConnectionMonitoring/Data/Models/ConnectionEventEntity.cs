namespace Data.Models;

/// <summary>
/// Connection event entity.
/// </summary>
public class ConnectionEventEntity
{
    /// <summary>
    /// Id.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Name of event.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Connection Id.
    /// </summary>
    public string? ConnectionId { get; set; }

    /// <summary>
    /// Time of the event.
    /// </summary>
    public DateTime EventTime { get; set; }
}
