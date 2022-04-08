using Core.Services;
using Microsoft.AspNetCore.Mvc;
using ConnectionInfo = Core.Models.ConnectionInfo;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConnectionInfoController : ControllerBase
{
    private readonly IConnectionInfoService _connectionInfoService;

    public ConnectionInfoController(IConnectionInfoService connectionInfoService)
    {
        _connectionInfoService = connectionInfoService;
    }

    [HttpGet]
    public async Task<ActionResult<ConnectionInfo[]>> GetAll()
    {
        var connectionsInfo = await _connectionInfoService.GetAllAsync();

        return Ok(connectionsInfo);
    }

    [HttpPost]
    public async Task<ActionResult> Save([FromBody] ConnectionInfo connectionInfo)
    {
        await _connectionInfoService.SaveAsync(connectionInfo);

        return Ok();
    }
}