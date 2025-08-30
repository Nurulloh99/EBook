using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Api.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        var bookGroup = app.MapGroup("/api/books")
            .RequireAuthorization()
            .WithTags("Book Management");

        // GET all books (with optional pagination)
        bookGroup.MapGet("/", [Authorize]
        async (int? skip, int? take, [FromServices] IBookService _bookService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;

            if (userId is null) throw new ArgumentNullException();
            GetPageModel<BookGetDto> books;
            PageModel pageModel;
            if (skip is null || take is null)
            {
                books = await _bookService.GetAllBooksAsync();
            }
            else
            {
                pageModel = new PageModel() { Skip = skip.Value, Take = take.Value, UserId = long.Parse(userId) };
                books = await _bookService.GetAllBooksAsync(pageModel);
            }
            return Results.Ok(books);
        })
        .WithName("GetAllBooks");

        // GET book by id
        bookGroup.MapGet("/{bookId:long}", [Authorize]
        async (long bookId, [FromServices] IBookService _bookService) =>
        {
            var book = await _bookService.GetBookByIdAsync(bookId);
            return Results.Ok(book);
        })
        .WithName("GetBookId");

        // GET books by language
        bookGroup.MapGet("/by-language/{languageId:long}", [Authorize]
        async (long languageId, [FromServices] IBookService _bookService) =>
        {
            var books = await _bookService.GetBooksByLanguageAsync(languageId);
            return Results.Ok(books);
        })
        .WithName("GetAllBooksByLanguage");

        // GET books by genre
        bookGroup.MapGet("/by-genre/{genreId:long}", [Authorize]
        async (long genreId, [FromServices] IBookService _bookService) =>
        {
            var books = await _bookService.GetBooksByGenreAsync(genreId);
            return Results.Ok(books);
        })
        .WithName("GetAllBooksByGenre");

        // GET search by keyword
        bookGroup.MapGet("/search", [Authorize]
        async (string keyword, [FromServices] IBookService _bookService) =>
        {
            var books = await _bookService.SearchByTitle(keyword);
            return Results.Ok(books);
        })
        .WithName("SearchBooks");
    }
}
