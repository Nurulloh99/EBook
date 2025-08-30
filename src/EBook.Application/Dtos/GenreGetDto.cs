namespace EBook.Application.Dtos;

public class GenreGetDto
{
    public long GenreId { get; set; }
    public string GenreName { get; set; }
    public string? GenreDescription { get; set; }

    public ICollection<BookGetDto> Books { get; set; }
}
