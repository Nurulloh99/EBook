namespace EBook.Application.Dtos;

public class GenreUpdateDto
{
    public long GenreId { get; set; }
    public string GenreName { get; set; }
    public string? GenreDescription { get; set; }
}
