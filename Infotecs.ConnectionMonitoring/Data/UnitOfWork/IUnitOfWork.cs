namespace Data.UnitOfWork;

/// <summary>
/// InterfaceUnitOfWork.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Begin transaction.
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// Commit changes.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rollback changes.
    /// </summary>
    void Rollback();
}
