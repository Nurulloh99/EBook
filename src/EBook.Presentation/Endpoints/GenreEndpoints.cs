using EBook.Application.Dtos;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Presentation.Endpoints;

public static class GenreEndpoints
{
    public static void MapGenreEndpoints(this WebApplication app)
    {
        var genreGroup = app.MapGroup("/api/genres")
            .RequireAuthorization()
            .WithTags("Genre Management");

        // GET genre by id
        genreGroup.MapGet("/{genreId:long}", [Authorize]
        async (long genreId, [FromServices] IGenreService _genreService) =>
        {
            var genre = await _genreService.GetGenreByIdAsync(genreId);
            return Results.Ok(genre);
        })
        .WithName("GetGenreById");

        // GET all genres
        genreGroup.MapGet("/", [Authorize]
        async ([FromServices] IGenreService _genreService) =>
        {
            var genres = await _genreService.GetAllGenresAsync();
            return Results.Ok(genres);
        })
        .WithName("GetAllGenres");

        // GET search genres
        genreGroup.MapGet("/search", [Authorize]
        async (string keyword, [FromServices] IGenreService _genreService) =>
        {
            var genres = await _genreService.SearchGenre(keyword);
            return Results.Ok(genres);
        })
        .WithName("SearchGenres");
    }
}
