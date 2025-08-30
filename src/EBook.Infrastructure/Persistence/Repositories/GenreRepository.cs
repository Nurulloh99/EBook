using EBook.Application.Dtos.Pagination;
using EBook.Application.Interfaces;
using EBook.Domain.Entities;
using EBook.Errors;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EBook.Infrastructure.Persistence.Repositories;

public class GenreRepository(AppDbContext _appDbContext) : IGenreRepository
{
    public async Task<long> InsertGenreAsync(Genre genre)
    {
        await _appDbContext.AddAsync(genre);
        await _appDbContext.SaveChangesAsync();
        return genre.GenreId;
    }

    public async Task RemoveGenreAsync(long genreId)
    {
        var genre = await _appDbContext.Genres.FirstOrDefaultAsync(o => o.GenreId == genreId);
        if (genre is null)
            throw new ArgumentNullException($"Genre not exists with this ID: {genreId} (Repository)");
        _appDbContext.Remove(genre);

        _appDbContext.SaveChanges();
    }

    public async Task<ICollection<Genre>> SelectAllGenresAsync()
    {
        var genre = await _appDbContext.Genres.AsNoTracking().ToListAsync();

        if (genre is null || genre.Count == 0)
            throw new EntityNotFoundException("No genre found. (Repository)");

        return genre;
    }

    public async Task<Genre> SelectGenreByIdAsync(long genreId)
    {
        var genre = await _appDbContext.Genres
            .Include(g => g.Books)
            .FirstOrDefaultAsync(o => o.GenreId == genreId);
        if (genre is null)
            throw new ArgumentNullException($"Genre not exists with this ID: {genreId} (Repository)");

        return genre;
    }

    public async Task UpdateGenreAsync(Genre genre)
    {
        _appDbContext.Genres.Update(genre);
        await _appDbContext.SaveChangesAsync();
    }
}
