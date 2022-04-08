using Core.Models;
using Core.Services;
using Microsoft.Extensions.Logging;

namespace Data.Services;

public class ConnectionInfoService : IConnectionInfoService
{
    public ConnectionInfoService(ILogger<ConnectionInfoService> logger)
    {
        this.logger = logger;
    }

    public Dictionary<string, ConnectionInfo> ConnectionsInfo { get; set; } = new();

    private readonly ILogger<ConnectionInfoService> logger;

    public Task<ConnectionInfo[]> GetAllAsync()
    {
        logger.LogInformation("Get all devices");

        return Task.FromResult(ConnectionsInfo.Values.ToArray());
    }

    public Task SaveAsync(ConnectionInfo connectionInfo)
    {
        if (connectionInfo.Id == null)
        {
            throw new Exception("Id con not be null");
        }

        if (ConnectionsInfo.ContainsKey(connectionInfo.Id))
        {
            ConnectionsInfo[connectionInfo.Id] = connectionInfo;
            logger.LogInformation($"Device with id {connectionInfo.Id} has been updated");
        }
        else
        {
            ConnectionsInfo.Add(connectionInfo.Id, connectionInfo);
            logger.LogInformation($"Device with id {connectionInfo.Id} has been added");
        }

        return Task.CompletedTask;
    }
}
