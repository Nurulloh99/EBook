using EBook.Application.Dtos;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/users")
            .RequireAuthorization()
            .WithTags("User Management");

        // PATCH update user
        userGroup.MapPatch("/{id:long}", [Authorize]
        async (UserGetDto userUpdateDto, [FromServices] IUserService userService) =>
        {
            await userService.UpdateUserAsync(userUpdateDto);
            return Results.Ok();
        })
        .WithName("UpdateUser");

        // GET user by username
        userGroup.MapGet("/by-username/{userName}", [Authorize]
        async (string userName, [FromServices] IUserService userService) =>
        {
            var user = await userService.GetUserByUserNameAsync(userName);
            return Results.Ok(user);
        })
        .WithName("GetUserByUserName");
    }
}
