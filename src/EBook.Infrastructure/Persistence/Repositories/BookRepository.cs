using EBook.Application.Dtos.Pagination;
using EBook.Application.Interfaces;
using EBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EBook.Infrastructure.Persistence.Repositories;

public class BookRepository(AppDbContext _appDbContext) : IBookRepository
{
    public async Task<long> InsertBookAsync(Book book)
    {
        await _appDbContext.AddAsync(book);
        await _appDbContext.SaveChangesAsync();
        return book.BookId;
    }

    public async Task RemoveBookAsync(long bookId)
    {
        var book = await _appDbContext.Books.FirstOrDefaultAsync(o => o.BookId == bookId);
        if (book is null)
            throw new ArgumentNullException($"Book not exists with this ID: {bookId} (Repository)");
        _appDbContext.Remove(book);

        _appDbContext.SaveChanges();
    }

    public async Task<ICollection<Book>> SelectAllBooksAsync(PageModel? pageModel)
    {
        var books = _appDbContext.Books
            .Where(c => c.UserId == pageModel.UserId)
            .OrderBy(c => c.BookId)
            .Skip(pageModel.Skip)
            .Take(pageModel.Take);

        var query = books.ToQueryString();
        return await books.ToListAsync();
    }

    public async Task<Book> SelectBookByIdAsync(long bookId)
    {
        var book = await _appDbContext.Books
            .Include(r => r.Reviews)
            .FirstOrDefaultAsync(o => o.BookId == bookId);

        if (book is null)
            throw new ArgumentNullException($"Book not exists with this ID: {bookId} (Repository)");

        return book;
    }

    public async Task<ICollection<Book>> SelectBooksByGenreAsync(long genreId)
    {
        var book = await _appDbContext.Books.Where(o => o.GenreId == genreId).ToListAsync();
        if (book is null)
            throw new ArgumentNullException($"Book not exists with this GenreID: {genreId} (Repository)");

        return book;
    }

    public async Task<ICollection<Book>> SelectBooksByLanguageAsync(long languageId)
    {
        var book = await _appDbContext.Books.Where(o => o.LanguageId == languageId).ToListAsync();
        if (book is null)
            throw new ArgumentNullException($"Book not exists with this LanguageID: {languageId} (Repository)");

        return book;
    }

    public async Task<ICollection<Book>> SelectBooksByUserIdAsync(long userId)
    {
        var book = await _appDbContext.Books.Where(o => o.UserId == userId).ToListAsync();
        if (book is null)
            throw new ArgumentNullException($"Book not exists with this UserID: {userId} (Repository)");

        return book;
    }

    public async Task UpdateBookAsync(Book book)
    {
        _appDbContext.Books.Update(book);
        await _appDbContext.SaveChangesAsync();
    }
}
