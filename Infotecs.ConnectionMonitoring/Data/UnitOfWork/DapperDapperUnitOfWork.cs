using System.Data;
using Data.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Data.UnitOfWork;

/// <summary>
/// UnitOfWork.
/// </summary>
public sealed class DapperUnitOfWork : IDapperUnitOfWork
{
    private IDbTransaction? transaction;
    private IDbConnection? connection;

    private IConnectionMonitoringRepository? connectionMonitoringRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DapperUnitOfWork"/> class.
    /// </summary>
    /// <param name="configuration">IConfiguration.</param>
    public DapperUnitOfWork(IConfiguration configuration)
    {
        connection = new NpgsqlConnection(configuration.GetConnectionString("InfotecsMonitoring"));
        connection.Open();
        transaction = connection.BeginTransaction();
    }

    /// <summary>
    /// ConnectionMonitoringRepository.
    /// </summary>
    public IConnectionMonitoringRepository ConnectionMonitoringRepository => connectionMonitoringRepository ?? (connectionMonitoringRepository = new ConnectionMonitoringRepository(transaction));
    
    /// <summary>
    /// Commit changes.
    /// </summary>
    public void Commit()
    {
        try
        {
            transaction?.Commit();
        }
        catch
        {
            transaction?.Rollback();
            throw;
        }
        finally
        {
            transaction?.Dispose();
            transaction = connection?.BeginTransaction();
            ResetRepositories();
        }
    }

    /// <summary>
    /// Rollback changes.
    /// </summary>
    public void Rollback()
    {
        transaction?.Rollback();
        Dispose();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (transaction != null)
        {
            transaction.Dispose();
            transaction = null;
        }

        if (connection != null)
        {
            connection.Dispose();
            connection = null;
        }
    }

    private void ResetRepositories()
    {
        connectionMonitoringRepository = null;
    }
}
