using EBook.Application.Dtos;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Presentation.Endpoints;

public static class ReviewEndpoint
{
    public static void MapReviewEndpoints(this WebApplication app)
    {
        var reviewGroup = app.MapGroup("/api/review")
            .RequireAuthorization()
            .WithTags("Review Management");


        reviewGroup.MapPost("/add-review", [Authorize]
        async (ReviewCreateDto reviewCreateDto, [FromServices] IReviewService _reviewService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();

            var reviewId = await _reviewService.AddReviewAsync(reviewCreateDto, long.Parse(userId));
            return Results.Ok(reviewId);
        })
            .WithName("CreateReview");


        reviewGroup.MapDelete("/delete-review", [Authorize]
        async (long reviewId, [FromServices] IReviewService _reviewService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();

            await _reviewService.DeleteReviewAsync(long.Parse(userId), reviewId);
            return Results.Ok(reviewId);
        })
            .WithName("DeleteReview");


        reviewGroup.MapGet("/get-review-by-id", [Authorize]
        async (long reviewId, [FromServices] IReviewService _reviewService) =>
        {
            var review = await _reviewService.GetReviewByIdAsync(reviewId);
            return Results.Ok(review);
        })
            .WithName("GetReviewById");


        reviewGroup.MapPatch("/change-review", [Authorize]
        async (ReviewGetDto reviewGetDto, [FromServices] IReviewService _reviewService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();

            await _reviewService.ChangeReviewAsync(reviewGetDto, long.Parse(userId));
            return Results.Ok(reviewGetDto);
        })
            .WithName("ChangeReview");


        reviewGroup.MapGet("/get-all-reviews-by-user-id", [Authorize]
        async ([FromServices] IReviewService _reviewService, HttpContext context) =>
        {
            var user = context.User.FindFirst("UserId")?.Value;
            if (user is null) throw new ArgumentNullException();

            var reviews = await _reviewService.GetReviewsByUserIdAsync(long.Parse(user));

            return Results.Ok(reviews);
        })
            .WithName("GetAllReviewsByUserId");


        reviewGroup.MapGet("/get-all-reviews-by-book-id", [Authorize]
        async (long bookId, [FromServices] IReviewService _reviewService) =>
        {
            var reviews = await _reviewService.GetReviewsByBookIdAsync(bookId);

            return Results.Ok(reviews);
        })
            .WithName("GetAllReviewsByBook");
    }
}
