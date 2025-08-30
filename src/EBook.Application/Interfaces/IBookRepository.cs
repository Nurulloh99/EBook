using EBook.Application.Dtos.Pagination;
using EBook.Domain.Entities;

namespace EBook.Application.Interfaces;

public interface IBookRepository
{
    Task<long> InsertBookAsync(Book book);
    Task<ICollection<Book>> SelectAllBooksAsync(PageModel? pageModel = null);
    Task<ICollection<Book>> SelectBooksByLanguageAsync(long languageId);
    Task<ICollection<Book>> SelectBooksByGenreAsync(long genreId);
    Task<ICollection<Book>> SelectBooksByUserIdAsync(long userId);
    Task<Book> SelectBookByIdAsync(long bookId);
    Task UpdateBookAsync(Book book);
    Task RemoveBookAsync(long bookId); 
}
