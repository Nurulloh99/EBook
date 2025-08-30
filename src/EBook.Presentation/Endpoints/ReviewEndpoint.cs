using EBook.Application.Dtos;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Presentation.Endpoints;

public static class ReviewEndpoint
{
    public static void MapReviewEndpoints(this WebApplication app)
    {
        var reviewGroup = app.MapGroup("/api/reviews")
            .RequireAuthorization()
            .WithTags("Review Management");

        // POST create review
        reviewGroup.MapPost("/", [Authorize]
        async (ReviewCreateDto reviewCreateDto, [FromServices] IReviewService _reviewService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();

            var reviewId = await _reviewService.AddReviewAsync(reviewCreateDto, long.Parse(userId));
            return Results.Created($"/api/reviews/{reviewId}", reviewId);
        })
        .WithName("CreateReview");

        // DELETE review by id
        reviewGroup.MapDelete("/{reviewId:long}", [Authorize]
        async (long reviewId, [FromServices] IReviewService _reviewService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();

            await _reviewService.DeleteReviewAsync(long.Parse(userId), reviewId);
            return Results.NoContent();
        })
        .WithName("DeleteReview");

        // GET review by id
        reviewGroup.MapGet("/{reviewId:long}", [Authorize]
        async (long reviewId, [FromServices] IReviewService _reviewService) =>
        {
            var review = await _reviewService.GetReviewByIdAsync(reviewId);
            return Results.Ok(review);
        })
        .WithName("GetReviewById");

        // PATCH update review
        reviewGroup.MapPatch("/{reviewId:long}", [Authorize]
        async (ReviewGetDto reviewGetDto, [FromServices] IReviewService _reviewService, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new ArgumentNullException();

            await _reviewService.ChangeReviewAsync(reviewGetDto, long.Parse(userId));
            return Results.Ok(reviewGetDto);
        })
        .WithName("ChangeReview");

        // GET reviews by current user
        reviewGroup.MapGet("/by-user/me", [Authorize]
        async ([FromServices] IReviewService _reviewService, HttpContext context) =>
        {
            var user = context.User.FindFirst("UserId")?.Value;
            if (user is null) throw new ArgumentNullException();

            var reviews = await _reviewService.GetReviewsByUserIdAsync(long.Parse(user));
            return Results.Ok(reviews);
        })
        .WithName("GetAllReviewsByUserId");

        // GET reviews by book id
        reviewGroup.MapGet("/by-book/{bookId:long}", [Authorize]
        async (long bookId, [FromServices] IReviewService _reviewService) =>
        {
            var reviews = await _reviewService.GetReviewsByBookIdAsync(bookId);
            return Results.Ok(reviews);
        })
        .WithName("GetAllReviewsByBook");
    }
}
