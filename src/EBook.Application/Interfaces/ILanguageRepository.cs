using EBook.Domain.Entities;

namespace EBook.Application.Interfaces;

public interface ILanguageRepository
{
    Task<long> InsertLanguageAsync(Language language);
    Task<ICollection<Language>> SelectAllLanguagesAsync();
    Task<Language> SelectLanguageByIdAsync(long languageId);
    Task UpdateLanguageAsync(Language language);
    Task RemoveLanguageAsync(long languageId);
}
