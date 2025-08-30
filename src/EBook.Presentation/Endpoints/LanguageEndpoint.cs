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
        var languageGroup = app.MapGroup("/api/language")
            .RequireAuthorization()
            .WithTags("Language Management");


        languageGroup.MapPost("/add-language", [Authorize]
        async (LanguageCreateDto languageCreateDto, [FromServices] ILanguageService _languageService) =>
        {
            var languageId = await _languageService.AddLanguageAsync(languageCreateDto);
            return Results.Ok(languageId);
        })
            .WithName("CreateLanguage");


        languageGroup.MapDelete("/delete-language", [Authorize]
        async (long languageId, [FromServices] ILanguageService _languageService) =>
        {
            await _languageService.DeleteLanguageAsync(languageId);
            return Results.Ok(languageId);
        })
            .WithName("DeleteLanguage");


        languageGroup.MapGet("/get-language-by-id", [Authorize]
        async (long languageId, [FromServices] ILanguageService _languageService) =>
        {
            var language = await _languageService.GetLanguageByIdAsync(languageId);
            return Results.Ok(language);
        })
            .WithName("GetLanguageById");


        languageGroup.MapGet("/get-all-languages", [Authorize]
        async ([FromServices] ILanguageService _languageService) =>
        {
            var languages = await _languageService.GetAllLanguagesAsync();
            return Results.Ok(languages);
        })
        .WithName("GetAllLanguages");
    }
}
