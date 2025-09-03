namespace EBook.Application.Dtos.Pagination;

public class PageModel
{

    public long? UserId { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 2000000000;
}

