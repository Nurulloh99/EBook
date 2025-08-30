using EBook.Application.Dtos;
using EBook.Domain.Entities;

namespace EBook.Application.Services;

public static class MapService
{
    public static User ConvertToUserEntity(UserCreateDto userCreateDto)
    {
        return new User
        {
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            UserName = userCreateDto.UserName,
            Email = userCreateDto.Email,
            Password = userCreateDto.Password,
            PhoneNumber = userCreateDto.PhoneNumber,
        };
    }

    public static UserGetDto ConvertToUserGetDto(User user)
    {
        return new UserGetDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = user.Role.RoleName,
        };
    }

    //===================================================================================================

    public static Role ConvertToRoleEntity(RoleCreateDto roleCreateDto)
    {
        return new Role
        {
            RoleName = roleCreateDto.RoleName,
            RoleDescription = roleCreateDto.RoleDescription
        };
    }

    public static RoleGetDto ConvertToRoleDto(Role role)
    {
        return new RoleGetDto
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
            RoleDescription = role.RoleDescription
        };
    }

    //===================================================================================================

    public static Review ConvertToReviewEntity(ReviewCreateDto reviewCreateDto)
    {
        return new Review
        {
            Content = reviewCreateDto.Content,
            Rating = reviewCreateDto.Rating,
            BookId = reviewCreateDto.BookId,    
        };
    }

    public static ReviewGetDto ConvertToReviewGetDto(Review review)
    {
        return new ReviewGetDto
        {
            ReviewId = review.ReviewId,
            Content = review.Content,
            Rating = review.Rating,
        };
    }

    //===================================================================================================

    public static Book ConvertToBookEntity(BookCreateDto bookCreateDto)
    {
        return new Book
        {
            Title = bookCreateDto.Title,
            Author = bookCreateDto.Author,
            Description = bookCreateDto.Description,
            Published = bookCreateDto.Published,
            Pages = bookCreateDto.Pages,
        };
    }

    public static BookGetDto ConvertToBookGetDto(Book book)
    {
        return new BookGetDto
        {
            BookId = book.BookId,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            Published = book.Published,
            Pages = book.Pages,
            BookUrl = book.BookUrl,
            ThumbnaliUrl = book.ThumbnaliUrl,
        };
    }

    public static BookDtoForById ConvertToBookDtoForById(Book book)
    {
        return new BookDtoForById
        {
            BookId = book.BookId,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            Published = book.Published,
            Pages = book.Pages,
            BookUrl = book.BookUrl,
            ThumbnaliUrl = book.ThumbnaliUrl,
            Reviews = book.Reviews == null ? new List<ReviewGetDto>() : book.Reviews.Select(ConvertToReviewGetDto).ToList()
        };
    }

    //===================================================================================================

    public static Language ConvertToLanguageEntity(LanguageCreateDto languageCreateDto)
    {
        return new Language
        {
            LanguageName = languageCreateDto.LanguageName,
        };
    }

    public static LanguageGetDto ConvertToLanguageGetDto(Language language)
    {
        return new LanguageGetDto
        {
            LanguageId = language.LanguageId,
            LanguageName = language.LanguageName,
        };
    }

    //===================================================================================================

    public static Genre ConvertToGenreEntity(GenreCreateDto bookCreateDto)
    {
        return new Genre
        {
            GenreName = bookCreateDto.GenreName,
            GenreDescription = bookCreateDto.GenreDescription,
        };
    }

    public static GenreGetDto ConvertToGenreGetDto(Genre book)
    {
        return new GenreGetDto
        {
            GenreId = book.GenreId,
            GenreName = book.GenreName,
            GenreDescription = book.GenreDescription,
            Books = book.Books is null ? new List<BookGetDto>() : book.Books.Select(ConvertToBookGetDto).ToList()
        };
    }

    public static GenreUpdateDto ConvertToGenreUpdateDto(Genre book)
    {
        return new GenreUpdateDto
        {
            GenreId = book.GenreId,
            GenreName = book.GenreName,
            GenreDescription = book.GenreDescription,
        };
    }

    //===================================================================================================
}
