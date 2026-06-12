namespace CRUD_EF_Core_API.Features.ObjBooks
{
    public record GetBooks(
            int BookId,
            string? Title,
            string? Author,
            DateOnly YearOfRead,
            int GenreId,
            string GenreName
        );
}
