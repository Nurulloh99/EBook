using EBook.Application.Dtos;
using EBook.Application.Interfaces;
using EBook.Application.Services.ServiceInterfaces;
using EBook.Domain.Entities;

namespace EBook.Application.Services.ServiceImplementations;


public class LanguageService(ILanguageRepository _languageRepo) : ILanguageService
{
    public async Task<long> AddLanguageAsync(LanguageCreateDto languageCreateDto)
    {
        return await _languageRepo.InsertLanguageAsync(new Language
        {
            LanguageName = languageCreateDto.LanguageName,
        });
    }

    public async Task DeleteLanguageAsync(long languageId)
    {
        await _languageRepo.RemoveLanguageAsync(languageId);
    }

    public async Task<ICollection<LanguageGetDto>> GetAllLanguagesAsync()
    {
        var languages = await _languageRepo.SelectAllLanguagesAsync();
        return languages.Select(MapService.ConvertToLanguageGetDto).ToList();
    }

    public async Task<LanguageGetDto> GetLanguageByIdAsync(long languageId)
    {
        return MapService.ConvertToLanguageGetDto(await _languageRepo.SelectLanguageByIdAsync(languageId));
    }
}
