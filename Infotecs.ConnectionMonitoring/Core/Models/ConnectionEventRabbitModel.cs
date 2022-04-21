using System.Text.Json.Serialization;

namespace Core.Models;

/// <summary>
/// Connection event model for Rabbit message.
/// </summary>
public class ConnectionEventRabbitModel
{
    /// <summary>
    /// Event Id.
    /// </summary>
    [JsonPropertyName("eventId")]
    public string? EventId { get; set; }

    /// <summary>
    /// Name of event.
    /// </summary>
    /// 
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Connection Id.
    /// </summary>
    /// 
    [JsonPropertyName("nodeId")]
    public string? NodeId { get; set; }

    /// <summary>
    /// Time of the event.
    /// </summary>
    /// 
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}
