using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// API for work with event.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConnectionEventController : ControllerBase
{
    private readonly IConnectionEventService connectionEventService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionEventController"/> class.
    /// </summary>
    /// <param name="connectionEventService">Service for work with ConnectionEvent.</param>
    public ConnectionEventController(IConnectionEventService connectionEventService)
    {
        this.connectionEventService = connectionEventService;
    }

    /// <summary>
    /// Get all events for current connection.
    /// </summary>
    /// <param name="connectionId">ConnectionId.</param>
    /// <returns>List of events.</returns>
    [HttpGet]
    public async Task<ActionResult<ConnectionEvent[]>> GetEventsByConnectionId(string connectionId)
    {
        ConnectionEvent[] connectionEvents = await connectionEventService.GetEventsByConnectionIdAsync(connectionId);

        return Ok(connectionEvents);
    }

    /// <summary>
    /// Create events.
    /// </summary>
    /// <param name="connectionEvents">Events list.</param>
    /// <returns>Task.</returns>
    [HttpPost]
    public async Task<ActionResult> Save([FromBody] IEnumerable<ConnectionEvent> connectionEvents)
    {
        await connectionEventService.SaveAsync(connectionEvents);

        return Ok();
    }
}
