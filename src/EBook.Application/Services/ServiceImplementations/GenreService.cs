using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;
using EBook.Application.Interfaces;
using EBook.Application.Services.ServiceInterfaces;
using EBook.Domain.Entities;
using EBook.Errors;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EBook.Application.Services.ServiceImplementations;

public class GenreService(IGenreRepository _genreRepository, ILogger<UserService> _logger) : IGenreService
{
    public async Task<long> AddGenreAsync(GenreCreateDto genreCreateDto)
    {
        if(genreCreateDto is null)
            throw new ArgumentNullException($"Genre is empty with this name of: {genreCreateDto} (Service)");
        var genre = MapService.ConvertToGenreEntity(genreCreateDto);

        return await _genreRepository.InsertGenreAsync(genre);
    }

    public async Task ChangeGenreAsync(GenreUpdateDto genreUpdateDto)
    {
        var genre = await _genreRepository.SelectGenreByIdAsync(genreUpdateDto.GenreId);
        if (genre is null)
            throw new ArgumentNullException($"Genre not exists with this ID: {genreUpdateDto.GenreId} (Service)");

        genre.GenreName = genreUpdateDto.GenreName;
        genre.GenreDescription = genreUpdateDto.GenreDescription;

        await _genreRepository.UpdateGenreAsync(genre);
    }

    public async Task DeleteGenreAsync(long genreId)
    {
        var genre = await _genreRepository.SelectGenreByIdAsync(genreId);
        await _genreRepository.RemoveGenreAsync(genre.GenreId);
    }

    public async Task<ICollection<GenreGetDto>> GetAllGenresAsync()
    {
        var allGenres = await _genreRepository.SelectAllGenresAsync();
        var items = allGenres.Select(MapService.ConvertToGenreGetDto).ToList();

        return items;
    }

    public async Task<GenreGetDto> GetGenreByIdAsync(long genreId)
    {
        var genre = await _genreRepository.SelectGenreByIdAsync(genreId);
        if (genre is null)
            throw new ArgumentNullException($"Genre not exists with this ID: {genreId} (Service)");

        var result = MapService.ConvertToGenreGetDto(genre);

        return result;
    }

    public async Task<List<GenreGetDto>> SearchGenre(string keyword)
    {
        var allGenres = await _genreRepository.SelectAllGenresAsync();

        var filtered = allGenres
            .Where(p => p.GenreName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .Select(MapService.ConvertToGenreGetDto).ToList();

        _logger.LogInformation($"SEARCHED genres by KEYWORD have been taken successfully at {DateTime.Now}");

        return filtered;
    }
}
