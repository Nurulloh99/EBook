namespace EBook.Domain.Entities;

public class Genre
{
    public long GenreId { get; set; }
    public string GenreName { get; set; }
    public string? GenreDescription { get; set; }

    public ICollection<Book> Books { get; set; }
}

