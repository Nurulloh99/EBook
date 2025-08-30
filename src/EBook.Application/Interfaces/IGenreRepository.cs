using EBook.Application.Dtos.Pagination;
using EBook.Domain.Entities;

namespace EBook.Application.Interfaces;

public interface IGenreRepository
{
    Task<long> InsertGenreAsync(Genre genre);
    Task<ICollection<Genre>> SelectAllGenresAsync();
    Task<Genre> SelectGenreByIdAsync(long genreId);
    Task UpdateGenreAsync(Genre genre);
    Task RemoveGenreAsync(long genreId);
}
