using EBook.Application.Dtos.Pagination;
using EBook.Domain.Entities;

namespace EBook.Application.Interfaces;

public interface IReviewRepository
{
    Task<long> InsertReviewAsync(Review review); // Yangi sharh qo'shish
    Task<List<Review>> SelectAllReviewsAsync();
    Task<Review> SelectReviewByIdAsync(long reviewId); // Sharhni ID bo'yicha olish
    Task<bool> SelectReviewByUserAndBookIdAsync(long bookId,long userId); 
    Task UpdateReviewAsync(Review review); // Sharhni yangilash
    Task RemoveReviewAsync(long reviewId); // Sharhni o'chirish
}
