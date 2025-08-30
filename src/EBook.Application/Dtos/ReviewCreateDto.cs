namespace EBook.Application.Dtos;

public class ReviewCreateDto
{
    public string Content { get; set; }
    public int Rating { get; set; }
    public long BookId { get; set; }
}
