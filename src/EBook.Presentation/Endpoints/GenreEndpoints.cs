using EBook.Application.Dtos;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Presentation.Endpoints;

public static class GenreEndpoints
{
    public static void MapGenreEndpoints(this WebApplication app)
    {
        var genreGroup = app.MapGroup("/api/genre")
            //.RequireAuthorization()
            .WithTags("Genre Management");


        genreGroup.MapPost("/add-genre", //[Authorize]
        async (GenreCreateDto genreCreateDto, [FromServices] IGenreService _genreService) =>
        {
            var genreId = await _genreService.AddGenreAsync(genreCreateDto);
            return Results.Ok(genreId);
        })
            .WithName("CreateGenre");


        genreGroup.MapDelete("/delete-genre", //[Authorize]
        async (long genreId, [FromServices] IGenreService _genreService) =>
        {
            await _genreService.DeleteGenreAsync(genreId);
            return Results.Ok(genreId);
        })
            .WithName("DeleteGenre");


        genreGroup.MapGet("/get-genre-by-id", //[Authorize]
        async (long genreId, [FromServices] IGenreService _genreService) =>
        {
            var genre = await _genreService.GetGenreByIdAsync(genreId);
            return Results.Ok(genre);
        })
            .WithName("GetGenreById");


        genreGroup.MapGet("/get-all-genres", //[Authorize]
        async ([FromServices] IGenreService _genreService) =>
        {
            var genres = await _genreService.GetAllGenresAsync();
            return Results.Ok(genres);
        })
        .WithName("GetAllGenres");


        genreGroup.MapPatch("/change-genre", //[Authorize]
        async (GenreUpdateDto genreUpdateDto, [FromServices] IGenreService _genreService) =>
        {
            await _genreService.ChangeGenreAsync(genreUpdateDto);
            return Results.Ok(genreUpdateDto);
        })
            .WithName("ChangeGenre");


        genreGroup.MapGet("/search-genres-by-keyword", //[Authorize]
        async (string keyword, [FromServices] IGenreService _genreService) =>
        {
            var genres = await _genreService.SearchGenre(keyword);
            return Results.Ok(genres);
        })
            .WithName("SearchGenres");
    }
}
