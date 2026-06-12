using Microsoft.EntityFrameworkCore;
using CRUD_EF_Core_API.Shared.Database;
using CRUD_EF_Core_API.Shared.Models;

namespace CRUD_EF_Core_API.Features.ObjBooks
{
    public class BooksServices(AppDbContext _db)
    {
        public async Task<IEnumerable<GetBooks>> GetAll()
        {
            var listbooks = await _db.Books.Include(g => g.Genre).ToListAsync();

            return listbooks.Select(e => new GetBooks(
                BookId: e.BookId,
                Title: e.Title,
                Author: e.Author,
                YearOfRead: e.YearOfRead,
                GenreId: e.GenreId,
                GenreName: e.Genre.Name
            )).ToList();
        }

        public async Task<GetBooks> GetById(int id)
        {
            var onebook = await _db.Books.Include(g => g.Genre).FirstOrDefaultAsync(e => e.BookId == id);

            if (onebook is null) return null;

            return new GetBooks(
                BookId: onebook.BookId,
                Title: onebook.Title,
                Author: onebook.Author,
                YearOfRead: onebook.YearOfRead,
                GenreId: onebook.GenreId,
                GenreName: onebook.Genre.Name
            );
        }

        public async Task Add(CreateBook request)
        {
            var newbook = new Books()
            {
                Title = request.Title,
                Author = request.Author,
                YearOfRead = request.YearOfRead,
                GenreId = request.GenreId
            };

            await _db.Books.AddAsync(newbook);
            await _db.SaveChangesAsync();
        }

        public async Task Update(UpdateBook request)
        {
            var bookfund = await _db.Books.FirstAsync(e => e.BookId == request.BookId);

            bookfund.Title = request.Title;
            bookfund.Author = request.Author;
            bookfund.YearOfRead = request.YearOfRead;
            bookfund.GenreId = request.GenreId;

            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var bookfund = await _db.Books.FirstAsync(e => e.BookId == id);
            _db.Books.Remove(bookfund);

            await _db.SaveChangesAsync();
        }
    }
}
