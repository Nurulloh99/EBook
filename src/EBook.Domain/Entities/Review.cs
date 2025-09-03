namespace EBook.Domain.Entities;

public class Review
{
    public long ReviewId { get; set; }
    public string Content { get; set; }
    public int? Rating { get; set; }   

    public long BookId { get; set; }
    public Book Book { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}
