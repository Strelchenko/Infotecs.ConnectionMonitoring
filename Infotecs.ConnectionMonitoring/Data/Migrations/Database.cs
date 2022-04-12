using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Data.Migrations;

/// <summary>
/// Class for creating Db.
/// </summary>
public class Database
{
    private readonly IConfiguration configuration;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Database"/> class.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    public Database(IConfiguration configuration) => this.configuration = configuration;

    /// <summary>
    /// Database creating.
    /// </summary>
    /// <param name="dbName">Name of database.</param>
    public void CreateDatabase(string dbName)
    {
        var query = "SELECT DATNAME FROM pg_catalog.pg_database WHERE DATNAME = @name";

        var queryArgs = new { name = dbName };

        using (var connection = new NpgsqlConnection(configuration.GetConnectionString("InfotecsMonitoring")))
        {
            IEnumerable<dynamic>? records = connection.Query(query, queryArgs);

            if (!records.Any())
                connection.Execute($"CREATE DATABASE {dbName}");
        }
    }
}
