using ProductCRUD.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProductCRUD.Infrastructure.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly Persistence.AppDbContext _ctx;
        private readonly IServiceProvider _provider;
        private readonly Dictionary<Type, object> _repos = new();
        private IDbContextTransaction? _tx;

        public EfUnitOfWork(Persistence.AppDbContext ctx, IServiceProvider provider)
        {
            _ctx = ctx;
            _provider = provider;
        }

        public IGenericRepository<T> Repository<T>() where T : ProductCRUD.Domain.Entities.Entity
        {
            if (_repos.TryGetValue(typeof(T), out var repo)) return (IGenericRepository<T>)repo!;
            var newRepo = new EfGenericRepository<T>(_ctx);
            _repos[typeof(T)] = newRepo!;
            return newRepo;
        }

        public Task<int> CommitAsync(System.Threading.CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);

        public async Task BeginTransactionAsync() => _tx = await _ctx.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            if (_tx != null)
            {
                await _tx.CommitAsync();
                await _tx.DisposeAsync();
                _tx = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_tx != null)
            {
                await _tx.RollbackAsync();
                await _tx.DisposeAsync();
                _tx = null;
            }
        }

        public void Dispose() => _ctx.Dispose();
    }
}
