using EBook.Application.Dtos;

namespace EBook.Application.Services.ServiceInterfaces;

public interface IReviewService
{
    Task<long> AddReviewAsync(ReviewCreateDto reviewCreateDto, long userId);
    Task<ReviewGetDto> GetReviewByIdAsync(long reviewId);
    Task ChangeReviewAsync(ReviewGetDto reviewUpdateDto, long userId);
    Task DeleteReviewAsync(long userId, long reviewId);

    Task<List<ReviewGetDto>> GetReviewsByUserIdAsync(long userId);
    Task<List<ReviewGetDto>> GetReviewsByBookIdAsync(long bookId);
}
    