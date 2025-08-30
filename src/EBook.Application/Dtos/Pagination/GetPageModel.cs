namespace EBook.Application.Dtos.Pagination;

public class GetPageModel<T>
{
    public PageModel PageModel { get; set; }
    public int TotalCount { get; set; }
    public List<T> Items { get; set; }
}
