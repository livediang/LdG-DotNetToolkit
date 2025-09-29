using ProductCRUD.Domain.Entities;
using ProductCRUD.Domain.Pagination;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCRUD.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : Entity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<PagedResult<T>> QueryAsync(PagedRequest request, Expression<Func<T, bool>>? filter = null, CancellationToken ct = default);
        Task AddAsync(T entity, CancellationToken ct = default);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
