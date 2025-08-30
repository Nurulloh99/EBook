using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EBook.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/role")
            .RequireAuthorization()
            .WithTags("User Management");


        userGroup.MapGet("/get-user-by-id", [Authorize(Roles = "SuperAdmin")]
        async (long userId, [FromServices] IUserService _userService) =>
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return Results.Ok(user);
        })
            .WithName("GetUserById");


        userGroup.MapDelete("/delete", [Authorize(Roles = "Admin, SuperAdmin")]
        async (long userId, HttpContext httpContext, [FromServices] IUserService userService) =>
        {
            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            await userService.DeleteUserAsync(userId, role);
            return Results.Ok();
        })
        .WithName("DeleteUser");


        userGroup.MapGet("/get-all-users",
        async (int? skip, int? take, [FromServices] IUserService userService) =>
        {
            GetPageModel<UserGetDto> users;
            PageModel pageModel;
            if (skip is null || take is null)
            {
                users = await userService.GetAllUsersAsync();
            }
            else
            {
                pageModel = new PageModel() { Skip = skip.Value, Take = take.Value };
                users = await userService.GetAllUsersAsync(pageModel);
            }

            return Results.Ok(users);
        })
            .WithName("GetAllUsers");


        userGroup.MapPatch("/update-user",
            async (UserGetDto userUpdateDto, [FromServices] IUserService userService) =>
            {
                await userService.UpdateUserAsync(userUpdateDto);
                return Results.Ok();
            })
            .WithName("UpdateUser");


        userGroup.MapPatch("/update-user-role", [Authorize(Roles = "SuperAdmin")]
        async (long userId, string userRole, [FromServices] IUserService userService) =>
                {
                    await userService.UpdateUserRoleAsync(userId, userRole);
                    return Results.Ok();
                })
                .WithName("UpdateUserRole");


        userGroup.MapGet("/get-all-users-by-role",
            async (string role, [FromServices] IUserService userService) =>
            {
                var users = await userService.GetAllUsersByRoleAsync(role);
                return Results.Ok(users);
            })
            .WithName("GetAllUsersByRole");


        userGroup.MapGet("/get-total-users-count-by-role", [Authorize(Roles = "SuperAdmin, Admin")]
        async (string role, [FromServices] IUserService userService) =>
            {
                var totalCount = await userService.GetTotalUsersCountByRoleAsync(role);
                return Results.Ok(totalCount);
            })
            .WithName("GetTotalUsersCountByRole");


        userGroup.MapGet("/get-user-by-username", [Authorize(Roles = "SuperAdmin, Admin")]
        async (string userName, [FromServices] IUserService userService) =>
                {
                    var user = await userService.GetUserByUserNameAsync(userName);
                    return Results.Ok(user);
                })
                .WithName("GetUserByUserName");


        userGroup.MapGet("/user-exists", [Authorize(Roles = "SuperAdmin, Admin")]
        async (long userId, [FromServices] IUserService userService) =>
            {
                var exists = await userService.UserExistsAsync(userId);
                return Results.Ok(exists);
            })
            .WithName("UserExists");


        userGroup.MapGet("/search-users-by-keyword",
            async (string keyword, [FromServices] IUserService userService) =>
            {
                var users = await userService.SearchUsersByKeywordAsync(keyword);
                return Results.Ok(users);
            })
            .WithName("SearchUsersByKeyword");
    }
}
