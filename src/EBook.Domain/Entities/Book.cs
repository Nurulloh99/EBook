namespace EBook.Domain.Entities;

public class Book
{
    public long BookId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } // Migration berish kere. GetAllBooks ga qoygan qoshimcha review larini GetById ga qoyishim kere. Genres ham qoshishim kere
    public string Author { get; set; }
    public DateOnly Published { get; set; }
    public int Pages { get; set; }

    public string BookUrl { get; set; }
    public string ThumbnaliUrl { get; set; }

    public long LanguageId { get; set; }
    public Language Language { get; set; }
 
    public long UserId { get; set; }
    public User User { get; set; }

    public long GenreId { get; set; }
    public Genre Genre { get; set; }

    public ICollection<Review> Reviews { get; set; }
}
