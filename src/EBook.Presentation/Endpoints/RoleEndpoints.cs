using Microsoft.AspNetCore.Authorization;
using EBook.Application.Dtos;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Api.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/role")
            //.RequireAuthorization()
            .WithTags("Role Management");


        userGroup.MapGet("/get-all-roles", //[Authorize(Roles = "SuperAdmin")]
        async ([FromServices] IRoleService _roleService) =>
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Results.Ok(roles);
        })
        .WithName("GetAllRoles");


        userGroup.MapPost("/add-role", //[Authorize(Roles = "SuperAdmin")]
        async (RoleCreateDto roleDto, [FromServices] IRoleService _roleService) =>
        {
            var roleId = await _roleService.AddRoleAsync(roleDto);
            return Results.Ok(roleId);
        })
        .WithName("AddRole");


        userGroup.MapDelete("/delete-role", //[Authorize(Roles = "SuperAdmin")]
        async (long roleId, [FromServices] IRoleService _roleService) =>
        {
            await _roleService.DeleteRoleAsync(roleId);
            return Results.Ok(roleId);
        })
        .WithName("DeleteRole");


        userGroup.MapGet("/get-role-by-id", //[Authorize(Roles = "SuperAdmin")]
        async (long roleId, [FromServices] IRoleService _roleService) =>
            {
                var role = await _roleService.GetRoleByIdAsync(roleId);
                return Results.Ok(role);
            })
            .WithName("GetRoleById");


        userGroup.MapPatch("/change-role", //[Authorize(Roles = "SuperAdmin")]
        async (RoleGetDto roleUpdateDto, [FromServices] IRoleService _roleService) =>
            {
                await _roleService.ChangeRoleAsync(roleUpdateDto);
                return Results.Ok(roleUpdateDto);
            })
            .WithName("ChangeRole");
    }
}

