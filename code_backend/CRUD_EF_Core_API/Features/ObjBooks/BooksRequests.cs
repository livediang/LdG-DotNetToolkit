namespace CRUD_EF_Core_API.Features.ObjBooks
{
    public record CreateBook(
            string Title,
            string Author,
            DateOnly YearOfRead,
            int GenreId
        );

    public record UpdateBook(
            int BookId,
            string Title,
            string Author,
            DateOnly YearOfRead,
            int GenreId
        );
}
