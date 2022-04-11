using Core.Services;
using Microsoft.AspNetCore.Mvc;
using ConnectionInfo = Core.Models.ConnectionInfo;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConnectionInfoController : ControllerBase
{
    private readonly IConnectionInfoService connectionInfoService;
    private readonly ILogger<ConnectionInfoController> logger;

    public ConnectionInfoController(IConnectionInfoService connectionInfoService, ILogger<ConnectionInfoController> logger)
    {
        this.connectionInfoService = connectionInfoService;
        this.logger = logger;
    }

    /// <summary>
    /// Get all connections
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<ConnectionInfo[]>> GetAll()
    {
        var connectionsInfo = await connectionInfoService.GetAllAsync();

        return Ok(connectionsInfo);
    }

    /// <summary>
    /// Create or update connection info
    /// </summary>
    /// <param name="connectionInfo"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> Save([FromBody] ConnectionInfo connectionInfo)
    {
        logger.LogInformation("Connection: {@ConnectionInfo}", connectionInfo);

        await connectionInfoService.SaveAsync(connectionInfo);

        return Ok();
    }
}
