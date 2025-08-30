using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;

namespace EBook.Application.Services.ServiceInterfaces;

public interface ILanguageService
{
    Task<long> AddLanguageAsync(LanguageCreateDto languageCreateDto);
    Task<ICollection<LanguageGetDto>> GetAllLanguagesAsync();
    Task<LanguageGetDto> GetLanguageByIdAsync(long languageId);
    Task DeleteLanguageAsync(long languageId);
}
