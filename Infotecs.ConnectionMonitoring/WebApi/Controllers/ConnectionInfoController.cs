using Core.Models;
using Core.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using ConnectionInfo = Core.Models.ConnectionInfo;

namespace WebApi.Controllers;

/// <summary>
/// API for work with connection.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConnectionInfoController : ControllerBase
{
    private readonly IConnectionInfoService connectionInfoService;
    private readonly ILogger<ConnectionInfoController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionInfoController"/> class.
    /// </summary>
    /// <param name="connectionInfoService">Service for work with ConnectionInfo.</param>
    /// <param name="logger">Logger.</param>
    public ConnectionInfoController(IConnectionInfoService connectionInfoService, ILogger<ConnectionInfoController> logger)
    {
        this.connectionInfoService = connectionInfoService;
        this.logger = logger;
    }

    /// <summary>
    /// Get all connections.
    /// </summary>
    /// <returns>List of connections.</returns>
    [HttpGet]
    public async Task<ActionResult<ConnectionInfo[]>> GetAll()
    {
        var connectionsInfo = await connectionInfoService.GetAllAsync();

        return Ok(connectionsInfo);
    }

    /// <summary>
    /// Create or update connection info.
    /// </summary>
    /// <param name="connectionInfo">Connection.</param>
    /// <returns>Task.</returns>
    [HttpPost]
    public async Task<ActionResult> Save([FromBody] ConnectionInfo connectionInfo)
    {
        logger.LogInformation("Connection: {@ConnectionInfo}", connectionInfo);

        await connectionInfoService.SaveAsync(connectionInfo);

        return Ok();
    }

    /// <summary>
    /// Create or update connection info with events.
    /// </summary>
    /// <param name="connectionInfoRequest">Connection with events.</param>
    /// <returns>Task.</returns>
    [HttpPost("addWithEvents")]
    public async Task<ActionResult> SaveWithEvents([FromBody] AddConnectionInfoRequest connectionInfoRequest)
    {
        logger.LogInformation("Connection: {@ConnectionInfo}", connectionInfoRequest);

        var connectionInfo = connectionInfoRequest.Adapt<ConnectionInfo>();
        var connectionEvents = connectionInfoRequest.Events.Adapt<IEnumerable<ConnectionEvent>>();

        await connectionInfoService.SaveAsync(connectionInfo, connectionEvents);

        return Ok();
    }
}
