namespace Data.Models;

public class ConnectionInfoEntity
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Os { get; set; }
    public string? AppVersion { get; set; }
    public DateTime LastConnection { get; set; }
}
