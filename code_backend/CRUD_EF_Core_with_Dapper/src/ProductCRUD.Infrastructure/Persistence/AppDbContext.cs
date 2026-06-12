using ProductCRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProductCRUD.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
                builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
                builder.Property(p => p.RowVersion).IsRowVersion();
                builder.HasQueryFilter(p => !p.IsDeleted); // filtro global soft-delete para Product
            });

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Domain.Entities.Entity>();
            foreach (var e in entries)
            {
                switch (e.State)
                {
                    case EntityState.Added:
                        e.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        e.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
