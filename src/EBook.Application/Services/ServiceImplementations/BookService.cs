using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;
using EBook.Application.Interfaces;
using EBook.Application.Services.ServiceInterfaces;
using EBook.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;

namespace EBook.Application.Services.ServiceImplementations;

public class BookService : IBookService
{
    IBookRepository _bookRepository;
    ILanguageRepository _languageRepository;
    IGenreRepository _genreRepository;
    ILogger<UserService> _logger;
    private readonly Cloudinary _cloudinary;
    public BookService(IConfiguration configuration, IGenreRepository genreRepository, IBookRepository bookRepository, ILanguageRepository languageRepository, ILogger<UserService> logger)
    {
        var cloudName = configuration["Cloudinary:CloudName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
        _bookRepository = bookRepository;
        _languageRepository = languageRepository;
        _genreRepository = genreRepository;
        _logger = logger;
    }


    private bool IsBook(IFormFile file)
    {
        // Ruxsat berilgan fayl kengaytmalari
        var allowedExtensions = new[] { ".pdf", ".epub", ".docx", ".txt", ".mobi" };

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return allowedExtensions.Contains(extension);
    }

    private bool IsImage(IFormFile file)
    {
        var allowedHeaders = new Dictionary<string, byte[]>
        {
            { "jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
            { "png", new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
            { "gif", new byte[] { 0x47, 0x49, 0x46, 0x38 } },
            { "bmp", new byte[] { 0x42, 0x4D } },
            { "webp", new byte[] { 0x52, 0x49, 0x46, 0x46 } }
        };

        using var reader = new BinaryReader(file.OpenReadStream());
        var fileHeader = reader.ReadBytes(4);

        return allowedHeaders.Any(h => fileHeader.Take(h.Value.Length).SequenceEqual(h.Value));
    }

    public async Task<string> UploadImageToCloudinaryAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Rasm fayli bo‘lishi shart.");

        if (!IsImage(file))
            throw new ArgumentException("Faqat rasm fayllar qabul qilinadi.");

        if (file.Length > 5 * 1024 * 1024)
            throw new ArgumentException("Rasm hajmi 5MB dan oshmasligi kerak.");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "photos"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Cloudinary upload failed: " + uploadResult.Error?.Message);
        }

        return uploadResult.SecureUrl.ToString();
    }


    public async Task<string> UploadBookAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Kitob fayli bo‘lishi shart.");

        if (!IsBook(file))
            throw new ArgumentException("Faqat kitob fayllar qabul qilinadi. (epub, pdf, docx, txt, mobi)");

        if (file.Length > 100 * 1024 * 1024)
            throw new ArgumentException("Kitob hajmi 100MB dan oshmasligi kerak.");

        await using var stream = file.OpenReadStream();

        var uploadParams = new RawUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "books",
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Cloudinary upload failed: " + uploadResult.Error?.Message);
        }

        var downloadUrl = uploadResult.SecureUrl.ToString()
    .Replace("/upload/", "/upload/fl_attachment/");
        return downloadUrl;
    }

    public async Task<long> AddBookAsync(long userId, BookCreateDto bookCreateDto)
    {
        if (bookCreateDto is null)
            throw new ArgumentNullException(nameof(bookCreateDto), "BookCreateDto cannot be null.");

        if (bookCreateDto.book == null)
            throw new ArgumentException("Book file must be provided.", nameof(bookCreateDto.book));

        var isBook = IsBook(bookCreateDto.book);
        if (!isBook)
        {
            throw new Exception("You can only upload book");
        }

        var book = MapService.ConvertToBookEntity(bookCreateDto);
        book.UserId = userId;
        book.LanguageId = bookCreateDto.LanguageId;
        book.GenreId = bookCreateDto.GenreId;
        book.BookUrl = await UploadBookAsync(bookCreateDto.book);
        book.ThumbnaliUrl = await UploadImageToCloudinaryAsync(bookCreateDto.Thumbnali);

        var bookId = await _bookRepository.InsertBookAsync(book);

        return bookId;
    }


    public async Task ChangeBookAsync(long userId, BookUpdateDto bookUpdateDto)
    {
        var book = await _bookRepository.SelectBookByIdAsync(bookUpdateDto.BookId);

        if (book.UserId != userId)
            throw new ForbiddenException($"This book {bookUpdateDto.BookId} is not depend on you!");

        if (book is null)
            throw new ArgumentNullException($"Book not exists with this ID: {bookUpdateDto.BookId} (Service)");

        book.Author = bookUpdateDto.Author;
        book.Title = bookUpdateDto.Title;
        book.Description = bookUpdateDto.Description;
        book.Published = bookUpdateDto.Published;
        book.Pages = bookUpdateDto.Pages;
        book.GenreId = bookUpdateDto.GenreId;
        book.LanguageId = bookUpdateDto.LanguageId; 

        _logger.LogInformation($"Selected Book with ID {bookUpdateDto.BookId} has been CHANGED successfully at {DateTime.Now}");

        await _bookRepository.UpdateBookAsync(book);
    }

    public async Task DeleteBookAsync(long userId, long bookId)
    {
        var book = await _bookRepository.SelectBookByIdAsync(bookId);

        if (book.UserId != userId)
            throw new ForbiddenException($"This book with this ID: {bookId} is not depend on you!");

        await _bookRepository.RemoveBookAsync(book.BookId);
    }

    public async Task<GetPageModel<BookGetDto>> GetAllBooksAsync(PageModel pageModel)
    {
        var allBooks = await _bookRepository.SelectAllBooksAsync(pageModel);
        var items = allBooks.Select(MapService.ConvertToBookGetDto).ToList();

        return new GetPageModel<BookGetDto>
        {
            PageModel = pageModel,
            TotalCount = items.Count,
            Items = items
        };
    }

    public async Task<BookDtoForById> GetBookByIdAsync(long bookId)
    {
        var book = await _bookRepository.SelectBookByIdAsync(bookId);
        if (book is null)
            throw new ArgumentNullException($"Book not exists with this ID: {bookId} (Service)");

        var result = MapService.ConvertToBookDtoForById(book);

        return result;
    }

    public async Task<List<BookGetDto>> GetBooksByLanguageAsync(long languageId)
    {
        var book = await _bookRepository.SelectBooksByLanguageAsync(languageId);

        var items = book.Select(MapService.ConvertToBookGetDto).ToList();

        return items;
    }

    public async Task<List<BookGetDto>> GetBooksByGenreAsync(long genreId)
    {
        var book = await _bookRepository.SelectBooksByGenreAsync(genreId);

        var items = book.Select(MapService.ConvertToBookGetDto).ToList();

        return items;
    }

    public async Task<List<BookGetDto>> GetBooksByUserIdAsync(long userId)
    {
        var result = await _bookRepository.SelectBooksByUserIdAsync(userId);

        return result.Select(MapService.ConvertToBookGetDto).ToList(); ;
    }


    public async Task<List<BookGetDto>> SearchByTitle(string keyword)
    {
        var allBooks = await _bookRepository.SelectAllBooksAsync();

        var filtered = allBooks
            .Where(p => p.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .Select(MapService.ConvertToBookGetDto).ToList();

        _logger.LogInformation($"SEARCHED books by KEYWORD have been taken successfully at {DateTime.Now}");

        return filtered;
    }
}
