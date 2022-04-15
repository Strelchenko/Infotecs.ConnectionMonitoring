namespace WebApi.Models;

/// <summary>
/// Model for adding ConnectionEvent.
/// </summary>
public class AddConnectionEventRequest
{
    /// <summary>
    /// Name of event.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Time of the event.
    /// </summary>
    public DateTime EventTime { get; set; }
}
