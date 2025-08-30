using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Api.Endpoints;

public static class BookEndpoints
{
    public record BookHelper(string Author, string Title, string Description, DateOnly Published, int Pages, long GenreId, long languageId);
    public static void MapBookEndpoints(this WebApplication app)
    {
        var bookGroup = app.MapGroup("/api/book")
            .RequireAuthorization()
            .WithTags("Book Management");


        bookGroup.MapPost("/add-book", [Authorize]
        async ([FromForm] BookHelper helper, IFormFile file, [FromServices] IBookService _bookService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();
            var bookCreateDto = new BookCreateDto
            {
                Author = helper.Author,
                Title = helper.Title,
                Description = helper.Description,
                Published = helper.Published,
                Pages = helper.Pages,
                LanguageId = helper.languageId,
                GenreId = helper.GenreId,
                book = file,
            };
            var bookId = await _bookService.AddBookAsync(long.Parse(userId), bookCreateDto);
            return Results.Ok(bookId);
        })
            .DisableAntiforgery()
            .WithName("CreateBook");


        bookGroup.MapDelete("/delete-book", [Authorize]
        async (long bookId, [FromServices] IBookService _bookService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();

            await _bookService.DeleteBookAsync(bookId, long.Parse(userId));
            return Results.Ok(bookId);
        })
            .WithName("DeleteBook");


        bookGroup.MapGet("/get-all-books", [Authorize]
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


        bookGroup.MapGet("/get-book-by-id", [Authorize]
        async (long bookId, [FromServices] IBookService _bookService) =>
        {
            var book = await _bookService.GetBookByIdAsync(bookId);
            return Results.Ok(book);
        })
            .WithName("GetBookId");


        bookGroup.MapPatch("/change-book", [Authorize]
        async (BookGetDto bookGetDto, [FromServices] IBookService _bookService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();

            await _bookService.ChangeBookAsync(long.Parse(userId), bookGetDto);
            return Results.Ok(bookGetDto);
        })
            .WithName("ChangeBook");


        bookGroup.MapGet("/get-all-books-by-user-id", [Authorize]
        async ([FromServices] IBookService _bookService, HttpContext context) =>
        {
            var user = context.User.FindFirst("UserId")?.Value;
            if (user is null) throw new ArgumentNullException();

            var books = await _bookService.GetBooksByUserIdAsync(long.Parse(user));

            return Results.Ok(books);
        })
            .WithName("GetAllBooksByUserId");


        bookGroup.MapGet("/get-all-books-by-language-id", [Authorize]
        async (long languageId, [FromServices] IBookService _bookService) =>
        {
            var books = await _bookService.GetBooksByLanguageAsync(languageId);

            return Results.Ok(books);
        })
            .WithName("GetAllBooksByLanguage");


        bookGroup.MapGet("/get-all-books-by-genre-id", [Authorize]
        async (long genreId, [FromServices] IBookService _bookService) =>
        {
            var books = await _bookService.GetBooksByGenreAsync(genreId);

            return Results.Ok(books);
        })
            .WithName("GetAllBooksByGenre");


        bookGroup.MapGet("/search-books-by-keyword", [Authorize]
        async (string keyword, [FromServices] IBookService _bookService) =>
        {
            var books = await _bookService.SearchByTitle(keyword);
            return Results.Ok(books);
        })
            .WithName("SearchBooks");
    }
}

