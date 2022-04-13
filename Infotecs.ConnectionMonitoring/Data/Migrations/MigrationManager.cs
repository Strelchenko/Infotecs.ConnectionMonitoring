using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Data.Migrations;

/// <summary>
/// Class for migration.
/// </summary>
public static class MigrationManager
{
    /// <summary>
    /// Db migration.
    /// </summary>
    /// <param name="host">Host.</param>
    /// <returns>Host.</returns>
    public static IHost MigrateDatabase<T>(this IHost host)
    {
        using (IServiceScope scope = host.Services.CreateScope())
        {
            var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
            var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<T>>();

            try
            {
                databaseService.CreateDatabase();

                migrationService.ListMigrations();
                migrationService.MigrateUp();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error migration");
                throw;
            }
        }

        return host;
    }
}
