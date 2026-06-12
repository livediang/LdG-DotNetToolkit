namespace CRUD_EF_Core_API.Features.ObjGenres
{
    public record GetGenre(
        int GenreId,
        string Name
    );

    public record CreateGenre(
        string Name
    );
}
