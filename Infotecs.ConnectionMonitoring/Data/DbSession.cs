using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Data;

/// <summary>
/// Class for creating db session.
/// </summary>
public sealed class DbSession : IDisposable
{
    /// <summary>
    /// Connection.
    /// </summary>
    public IDbConnection Connection { get; }

    /// <summary>
    /// Transaction.
    /// </summary>
    public IDbTransaction? Transaction { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbSession"/> class.
    /// </summary>
    /// <param name="configuration">IConfiguration.</param>
    public DbSession(IConfiguration configuration)
    {
        Connection = new NpgsqlConnection(configuration.GetConnectionString("InfotecsMonitoring"));
        Connection.Open();
    }

    /// <inheritdoc/>
    public void Dispose() => Connection?.Dispose();
}
