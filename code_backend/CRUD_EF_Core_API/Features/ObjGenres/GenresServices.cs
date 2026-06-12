using CRUD_EF_Core_API.Features.ObjBooks;
using CRUD_EF_Core_API.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace CRUD_EF_Core_API.Features.ObjGenres
{
    public class GenresServices(AppDbContext _db)
    {
        public async Task<IEnumerable<GetGenre>> GetAll()
        {
            var listgenres = await _db.Genre.ToListAsync();

            return listgenres.Select(e => new GetGenre(
                GenreId: e.GenreId,
                Name: e.Name
            )).ToList();
        }
    }
}
