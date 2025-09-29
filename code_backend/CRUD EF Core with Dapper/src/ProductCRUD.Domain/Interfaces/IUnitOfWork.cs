using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCRUD.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : Entities.Entity;
        Task<int> CommitAsync(CancellationToken ct = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
