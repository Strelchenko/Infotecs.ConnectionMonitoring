namespace Data.UnitOfWork;

/// <summary>
/// InterfaceUnitOfWork.
/// </summary>
public interface IDapperUnitOfWork : IDisposable
{
    /// <summary>
    /// Commit changes.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rollback changes.
    /// </summary>
    void Rollback();
}
