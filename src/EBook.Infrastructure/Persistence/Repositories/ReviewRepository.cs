using EBook.Application.Interfaces;
using EBook.Domain.Entities;
using EBook.Errors;
using Microsoft.EntityFrameworkCore;

namespace EBook.Infrastructure.Persistence.Repositories;

public class ReviewRepository(AppDbContext _appDbContext) : IReviewRepository
{
    public async Task<long> InsertReviewAsync(Review review)
    {
        await _appDbContext.AddAsync(review);
        await _appDbContext.SaveChangesAsync();
        return review.ReviewId;
    }

    public async Task RemoveReviewAsync(long reviewId)
    {
        var review = await _appDbContext.Reviews.FirstOrDefaultAsync(o => o.ReviewId == reviewId);
        if (review is null)
            throw new ArgumentNullException($"Review not exists with this ID: {reviewId} (Repository)");
        _appDbContext.Remove(review);

        _appDbContext.SaveChanges();
    }

    public async Task<List<Review>> SelectAllReviewsAsync()
    {
        var reviews = await _appDbContext.Reviews.AsNoTracking().ToListAsync();

        if (reviews is null || reviews.Count == 0)
            throw new EntityNotFoundException("No reviews found. (Repository)");

        return reviews;
    }

    public async Task<Review> SelectReviewByIdAsync(long reviewId)
    {
        var review = await _appDbContext.Reviews.FirstOrDefaultAsync(o => o.ReviewId == reviewId);
        if (review is null)
            throw new ArgumentNullException($"Review not exists with this ID: {reviewId} (Repository)");

        return review;
    }

    public async Task UpdateReviewAsync(Review review)
    {
        _appDbContext.Reviews.Update(review);
        await _appDbContext.SaveChangesAsync();
    }
}
