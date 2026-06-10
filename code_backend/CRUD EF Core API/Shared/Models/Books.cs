using System.ComponentModel.DataAnnotations;

namespace CRUD_EF_Core_API.Shared.Models
{
    public class Books
    {
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateOnly YearOfRead { get; set; }

        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
