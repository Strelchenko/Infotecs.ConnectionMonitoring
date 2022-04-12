namespace Core.Models;

/// <summary>
/// Connection info.
/// </summary>
public class ConnectionInfo
{
    /// <summary>
    /// Id.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// User name.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Type of OS.
    /// </summary>
    public string? Os { get; set; }

    /// <summary>
    /// Application version.
    /// </summary>
    public string? AppVersion { get; set; }

    /// <summary>
    /// Time of the last connection.
    /// </summary>
    public DateTime LastConnection { get; set; } = DateTime.UtcNow;
}
