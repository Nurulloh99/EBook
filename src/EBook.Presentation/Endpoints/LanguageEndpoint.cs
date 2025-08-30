using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Presentation.Endpoints;

public static class LanguageEndpoint
{
    public static void MapLanguageEndpoints(this WebApplication app)
    {
        var languageGroup = app.MapGroup("/api/languages")
            .RequireAuthorization()
            .WithTags("Language Management");

        // GET language by id
        languageGroup.MapGet("/{languageId:long}", [Authorize]
        async (long languageId, [FromServices] ILanguageService _languageService) =>
        {
            var language = await _languageService.GetLanguageByIdAsync(languageId);
            return Results.Ok(language);
        })
        .WithName("GetLanguageById");

        // GET all languages
        languageGroup.MapGet("/", [Authorize]
        async ([FromServices] ILanguageService _languageService) =>
        {
            var languages = await _languageService.GetAllLanguagesAsync();
            return Results.Ok(languages);
        })
        .WithName("GetAllLanguages");
    }
}
