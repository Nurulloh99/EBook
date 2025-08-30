using EBook.Application.Interfaces;
using EBook.Domain.Entities;
using EBook.Errors;
using Microsoft.EntityFrameworkCore;

namespace EBook.Infrastructure.Persistence.Repositories;

public class LanguageRepository(AppDbContext _appDbContext) : ILanguageRepository
{
    public async Task<long> InsertLanguageAsync(Language language)
    {
        await _appDbContext.AddAsync(language);
        await _appDbContext.SaveChangesAsync();
        return language.LanguageId;
    }

    public async Task RemoveLanguageAsync(long languageId)
    {
        var language = await _appDbContext.Languages.FirstOrDefaultAsync(o => o.LanguageId == languageId);
        if (language is null)
            throw new ArgumentNullException($"Language not exists with this ID: {languageId} (Repository)");
        _appDbContext.Remove(language);

        _appDbContext.SaveChanges();
    }

    public async Task<ICollection<Language>> SelectAllLanguagesAsync()
    {
        var language = await _appDbContext.Languages.AsNoTracking().ToListAsync();

        if (language is null || language.Count == 0)
            throw new EntityNotFoundException("No language found. (Repository)");

        return language;
    }

    public async Task<Language> SelectLanguageByIdAsync(long languageId)
    {
        var language = await _appDbContext.Languages.FirstOrDefaultAsync(o => o.LanguageId == languageId);
        if (language is null)
            throw new ArgumentNullException($"Language not exists with this ID: {languageId} (Repository)");

        return language;
    }

    public async Task UpdateLanguageAsync(Language language)
    {
        _appDbContext.Languages.Update(language);
        await _appDbContext.SaveChangesAsync();
    }
}
