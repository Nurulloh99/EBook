using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;

namespace EBook.Application.Services.ServiceInterfaces;

public interface IBookService
{
    Task<long> AddBookAsync(long userId, BookCreateDto bookCreateDto); // implementatsiya qismiiniyam user id si qoshilganligini polniy togirlash kere
    Task<GetPageModel<BookGetDto>> GetAllBooksAsync(PageModel pageModel = null);
    Task<BookDtoForById> GetBookByIdAsync(long bookId);
    Task ChangeBookAsync(long userId, BookUpdateDto bookUpdateDto);
    Task DeleteBookAsync(long userId, long bookId);

    Task<List<BookGetDto>> GetBooksByUserIdAsync(long userId);
    Task<List<BookGetDto>> GetBooksByLanguageAsync(long languageId);
    Task<List<BookGetDto>> GetBooksByGenreAsync(long genreId);
    Task<List<BookGetDto>> SearchByTitle(string keyword);
}
