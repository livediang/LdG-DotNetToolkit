using CRUD_EF_Core_API.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_EF_Core_API.Shared.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Books> Books { get; set; }
        public DbSet<Genre> Genre { get; set; }
    }
}
