namespace EBook.Application.Dtos;

public class BookUpdateDto
{
    public long BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public DateOnly Published { get; set; }
    public int Pages { get; set; }
    public long GenreId { get; set; }
    public long LanguageId { get; set; }
}
