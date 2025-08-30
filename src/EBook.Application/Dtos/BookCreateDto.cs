using Microsoft.AspNetCore.Http;

namespace EBook.Application.Dtos;

public class BookCreateDto
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public DateOnly Published { get; set; }
    public int Pages { get; set; }
    public IFormFile book { get; set; }

    public long LanguageId { get; set; }
    public long GenreId { get; set; }
}
