namespace Data.UnitOfWork;

/// <summary>
/// UnitOfWork.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbSession session;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="session">Session.</param>
    public UnitOfWork(DbSession session)
    {
        this.session = session;
    }

    /// <summary>
    /// Begin transaction.
    /// </summary>
    public void BeginTransaction()
    {
        session.Transaction = session.Connection.BeginTransaction();
    }

    /// <summary>
    /// Commit changes.
    /// </summary>
    public void Commit()
    {
        session.Transaction?.Commit();
        Dispose();
    }

    /// <summary>
    /// Rollback changes.
    /// </summary>
    public void Rollback()
    {
        session.Transaction?.Rollback();
        Dispose();
    }

    /// <inheritdoc/>
    public void Dispose() => session.Transaction?.Dispose();
}
