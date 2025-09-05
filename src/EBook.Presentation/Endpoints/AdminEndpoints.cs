using EBook.Application.Dtos;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EBook.Presentation.Endpoints;

public static class AdminEndpoints
{
    public record BookHelper(string Author,
        string Title, string Description,
        DateOnly Published, int Pages,
        long GenreId, long languageId);

    public static void MapAdminEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/admin")
                   .RequireAuthorization()
                   .WithTags("Admin Management");

        // ---------- Languages ----------
        userGroup.MapPost("/languages", [Authorize(Roles = "Admin, SuperAdmin")]
        async (LanguageCreateDto languageCreateDto, [FromServices] ILanguageService _languageService) =>
        {
            var languageId = await _languageService.AddLanguageAsync(languageCreateDto);
            return Results.Created($"/api/admin/languages/{languageId}", languageId);
        }).WithName("CreateLanguage");

        userGroup.MapDelete("/languages/{languageId:long}", [Authorize(Roles = "Admin, SuperAdmin")]
        async (long languageId, [FromServices] ILanguageService _languageService) =>
        {
            await _languageService.DeleteLanguageAsync(languageId);
            return Results.NoContent();
        }).WithName("DeleteLanguage");

        // ---------- Genres ----------
        userGroup.MapPost("/genres", [Authorize(Roles = "Admin, SuperAdmin")]
        async (GenreCreateDto genreCreateDto, [FromServices] IGenreService _genreService) =>
        {
            var genreId = await _genreService.AddGenreAsync(genreCreateDto);
            return Results.Created($"/api/admin/genres/{genreId}", genreId);
        }).WithName("CreateGenre");

        userGroup.MapPatch("/genres/{id:long}", [Authorize(Roles = "Admin, SuperAdmin")]
        async (GenreUpdateDto genreUpdateDto, [FromServices] IGenreService _genreService) =>
        {
            await _genreService.ChangeGenreAsync(genreUpdateDto);
            return Results.Ok(genreUpdateDto);
        }).WithName("ChangeGenre");

        userGroup.MapDelete("/genres/{genreId:long}", [Authorize(Roles = "Admin, SuperAdmin")]
        async (long genreId, [FromServices] IGenreService _genreService) =>
        {
            await _genreService.DeleteGenreAsync(genreId);
            return Results.NoContent();
        }).WithName("DeleteGenre");


        // ---------- Books ----------
        userGroup.MapPost("/books", [Authorize(Roles = "Admin, SuperAdmin")]
        async ([FromForm] BookHelper helper, IFormFile book, IFormFile image, [FromServices] IBookService _bookService, HttpContext context) =>
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
                book = book,
                Thumbnali = image,
            };
            var bookId = await _bookService.AddBookAsync(long.Parse(userId), bookCreateDto);
            return Results.Created($"/api/admin/books/{bookId}", bookId);
        }).DisableAntiforgery()
          .WithName("CreateBook");

        userGroup.MapPut("/books/{id:long}", [Authorize(Roles = "Admin, SuperAdmin")]
        async (BookUpdateDto bookGetDto, [FromServices] IBookService _bookService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();

            await _bookService.ChangeBookAsync(long.Parse(userId), bookGetDto);
            return Results.Ok(bookGetDto);
        }).WithName("ChangeBook");

        userGroup.MapDelete("/books/{bookId:long}", [Authorize(Roles = "Admin, SuperAdmin")]
        async (long bookId, [FromServices] IBookService _bookService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();

            await _bookService.DeleteBookAsync(bookId, long.Parse(userId));
            return Results.NoContent();
        }).WithName("DeleteBook");

        userGroup.MapGet("/users/me/books", [Authorize(Roles = "Admin, SuperAdmin")]
        async ([FromServices] IBookService _bookService, HttpContext context) =>
        {
            var user = context.User.FindFirst("UserId")?.Value;
            if (user is null) throw new ArgumentNullException();

            var books = await _bookService.GetBooksByUserIdAsync(long.Parse(user));
            return Results.Ok(books);
        }).WithName("GetAllBooksByUserId");


        // ---------- Roles ----------
        userGroup.MapPost("/roles", [Authorize(Roles = "SuperAdmin")]
        async (RoleCreateDto roleDto, [FromServices] IRoleService _roleService) =>
        {
            var roleId = await _roleService.AddRoleAsync(roleDto);
            return Results.Created($"/api/admin/roles/{roleId}", roleId);
        }).WithName("AddRole");

        userGroup.MapGet("/roles", [Authorize(Roles = "SuperAdmin")]
        async (IRoleService _roleService) =>
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Results.Ok(roles);
        }).WithName("GetAllRoles");

        userGroup.MapGet("/roles/{roleId:long}", [Authorize(Roles = "SuperAdmin, Admin")]
        async (long roleId, [FromServices] IRoleService _roleService) =>
        {
            var role = await _roleService.GetRoleByIdAsync(roleId);
            return Results.Ok(role);
        }).WithName("GetRoleById");

        userGroup.MapDelete("/roles/{roleId:long}", [Authorize(Roles = "SuperAdmin")]
        async (long roleId, [FromServices] IRoleService _roleService) =>
        {
            await _roleService.DeleteRoleAsync(roleId);
            return Results.NoContent();
        }).WithName("DeleteRole");

        userGroup.MapPatch("/roles/{userId:long}", [Authorize(Roles = "SuperAdmin")]
        async (long userId, string userRole, IUserService userService) =>
        {
            await userService.UpdateUserRoleAsync(userId, userRole);
            return Results.Ok();
        }).WithName("UpdateUserRole");


        // ---------- Users ----------
        userGroup.MapGet("/users/by-role/{role}", [Authorize(Roles = "SuperAdmin")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any, NoStore = false)]
        async (string role, IUserService _userService) =>
        {
            var users = await _userService.GetAllUsersByRoleAsync(role);
            return Results.Ok(new { success = true, data = users });
        }).WithName("GetAllUsersByRole");

        userGroup.MapDelete("/users/{userId:long}", [Authorize(Roles = "Admin, SuperAdmin")]
        async (long userId, HttpContext httpContext, IUserService userService) =>
        {
            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            await userService.DeleteUserAsync(userId, role);
            return Results.NoContent();
        }).WithName("DeleteUser");
    }
}
