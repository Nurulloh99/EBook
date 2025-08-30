using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;

namespace EBook.Application.Services.ServiceInterfaces;

public interface IGenreService
{
    Task<long> AddGenreAsync(GenreCreateDto genreCreateDto);
    Task<ICollection<GenreGetDto>> GetAllGenresAsync();
    Task<GenreGetDto> GetGenreByIdAsync(long genreId);
    Task ChangeGenreAsync(GenreUpdateDto genreUpdateDto);
    Task DeleteGenreAsync(long genreId);
    Task<List<GenreGetDto>> SearchGenre(string keyword);
}
