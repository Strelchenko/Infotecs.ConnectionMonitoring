using Core.Models;

namespace Core.Services;

/// <summary>
/// ConnectionEventService interface.
/// </summary>
public interface IConnectionEventService
{
    /// <summary>
    /// Get all events for current connection.
    /// </summary>
    /// <param name="connectionId">ConnectionId.</param>
    /// <returns>List of events.</returns>
    Task<ConnectionEvent[]> GetEventsByConnectionIdAsync(string connectionId);

    /// <summary>
    /// Create events.
    /// </summary>
    /// <param name="connectionEvents">Events list.</param>
    /// <returns>Task.</returns>
    Task SaveAsync(IEnumerable<ConnectionEvent> connectionEvents);
}
