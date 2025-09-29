using Microsoft.EntityFrameworkCore;
using ProductCRUD.Domain.Entities;
using ProductCRUD.Domain.Interfaces;
using ProductCRUD.Domain.Pagination;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCRUD.Infrastructure.Repositories
{
    public class EfGenericRepository<T> : IGenericRepository<T> where T : Entity
    {
        private readonly Persistence.AppDbContext _ctx;
        private readonly DbSet<T> _set;

        public EfGenericRepository(Persistence.AppDbContext ctx)
        {
            _ctx = ctx;
            _set = ctx.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id && !EF.Property<bool>(e, nameof(Entity.IsDeleted)), ct);

        public async Task<PagedResult<T>> QueryAsync(PagedRequest request, Expression<Func<T, bool>>? filter = null, CancellationToken ct = default)
        {
            IQueryable<T> q = _set.AsQueryable();

            // aplicar filtro de soft-delete si no está en modelo (aunque el HasQueryFilter lo maneja)
            q = q.Where(e => !EF.Property<bool>(e, nameof(Entity.IsDeleted)));

            if (filter != null) q = q.Where(filter);

            // search (si lo quieres ampliar, hazlo por propiedades concretas)
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                // Ejemplo simple: si T tuviera Name
            }

            // orden dinámico
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                var property = typeof(T).GetProperty(request.SortBy,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property != null)
                {
                    q = request.SortDesc
                        ? q.OrderByDescending(x => property.GetValue(x, null))
                        : q.OrderBy(x => property.GetValue(x, null));
                }
            }
            else
            {
                q = q.OrderByDescending(e => e.CreatedAt);
            }

            var total = await q.CountAsync(ct);
            var items = await q.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).AsNoTracking().ToListAsync(ct);

            return new ProductCRUD.Domain.Pagination.PagedResult<T>
            {
                Items = items,
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task AddAsync(T entity, CancellationToken ct = default) => await _set.AddAsync(entity, ct);

        public void Update(T entity) => _set.Update(entity);

        public void Remove(T entity) => _set.Remove(entity);

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
    }
}
