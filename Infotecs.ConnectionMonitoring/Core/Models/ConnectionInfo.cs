namespace Core.Models;

/// <summary>
/// Connection info
/// </summary>
public class ConnectionInfo
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Os { get; set; }
    public string? AppVersion { get; set; }
    public DateTime LastConnection { get; set; } = DateTime.UtcNow;
}
