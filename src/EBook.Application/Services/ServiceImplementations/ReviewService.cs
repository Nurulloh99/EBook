using EBook.Application.Dtos;
using EBook.Application.Interfaces;
using EBook.Application.Services.ServiceInterfaces;
using EBook.Domain.Entities;
using EBook.Errors;
using Microsoft.Extensions.Logging;

namespace EBook.Application.Services.ServiceImplementations;


public class ReviewService(IReviewRepository _reviewRepository, ILogger<UserService> _logger) : IReviewService
{
    public async Task<long> AddReviewAsync(ReviewCreateDto reviewCreateDto, long userId)
    {
        if (reviewCreateDto is null)
            throw new ArgumentNullException($"Review is empty with this name of: {reviewCreateDto} (Service)");
        var review = MapService.ConvertToReviewEntity(reviewCreateDto);
        review.UserId = userId;
        return await _reviewRepository.InsertReviewAsync(review);
    }

    public async Task ChangeReviewAsync(ReviewGetDto reviewUpdateDto, long userId)
    {
        var review = await _reviewRepository.SelectReviewByIdAsync(reviewUpdateDto.ReviewId);
        if (review is null)
            throw new ArgumentNullException($"Review not exists with this ID: {reviewUpdateDto.ReviewId} (Service)");

        review.Rating = reviewUpdateDto.Rating;
        review.Content = reviewUpdateDto.Content;
        review.UserId = userId;
        _logger.LogInformation($"Selected Review with ID {reviewUpdateDto.ReviewId} has been CHANGED successfully at {DateTime.Now}");

        await _reviewRepository.UpdateReviewAsync(review);
    }

    public async Task DeleteReviewAsync(long userId, long reviewId)
    {
        var review = await _reviewRepository.SelectReviewByIdAsync(reviewId);

        if (review.UserId != userId)
            throw new ForbiddenException($"This review with this ID: {reviewId} is not depend on you!");

        await _reviewRepository.RemoveReviewAsync(review.ReviewId);
    }

    public async Task<ICollection<ReviewGetDto>> GetAllReviewsAsync()
    {
        var allReviews = await _reviewRepository.SelectAllReviewsAsync();
        var items = allReviews.Select(MapService.ConvertToReviewGetDto).ToList();

        _logger.LogInformation($"Selected Reviews have been taken successfully at {DateTime.Now}");

        return items;
    }

    public async Task<ReviewGetDto> GetReviewByIdAsync(long reviewId)
    {
        var review = await _reviewRepository.SelectReviewByIdAsync(reviewId);
        if (review is null)
            throw new ArgumentNullException($"Review not exists with this ID: {reviewId} (Service)");

        var result = MapService.ConvertToReviewGetDto(review);

        _logger.LogInformation($"Selected Review by ID has been taken successfully at {DateTime.Now}");

        return result;
    }

    public async Task<List<ReviewGetDto>> GetReviewsByBookIdAsync(long bookId)
    {
        var allReviews = await _reviewRepository.SelectAllReviewsAsync();
        var filtered = allReviews.Where(p => p.BookId == bookId).Select(MapService.ConvertToReviewGetDto).ToList();

        if (filtered is null || filtered.Count == 0)
            throw new ArgumentNullException("No reviews found. (Service)");

        return filtered;
    }

    public async Task<List<ReviewGetDto>> GetReviewsByUserIdAsync(long userId)
    {
        var allReviews = await _reviewRepository.SelectAllReviewsAsync();
        var filtered = allReviews.Where(p => p.UserId == userId).Select(MapService.ConvertToReviewGetDto).ToList();

        _logger.LogInformation($"Selected Reviews by User Id have been taken successfully at {DateTime.Now}");

        if (filtered is null || filtered.Count == 0)
            throw new ArgumentNullException("No reviews found. (Service)");

        return filtered;
    }
}
