using EBook.Application.Dtos;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("auth")
            .WithTags("Auth Management");


        userGroup.MapPost("SignUp",
        async (UserCreateDto userDto, [FromServices] IAuthService authService) =>
        {
            var result = await authService.SignUpUserAsync(userDto);
            return Results.Ok(result);
        })
            .WithName("SignUp");


        userGroup.MapPost("/send-code",
            async (string email, [FromServices] IAuthService _service) =>
            {
                await _service.EailCodeSender(email);
            })
            .WithName("SendCode");


        userGroup.MapPost("/confirm-code",
            async (string code, string email, [FromServices] IAuthService _service) =>
            {
                var res = await _service.ConfirmCode(code, email);
                return Results.Ok(res);
            })
            .WithName("ConfirmCode");


        userGroup.MapPost("Login",
            async (LoginDto userLoginDto, [FromServices] IAuthService authService) => // RefreshToken bolishi kere
            {
                return Results.Ok(await authService.LoginUserAsync(userLoginDto /*refreshToken bolishi kere*/));
            })
                .WithName("Login");


        userGroup.MapPut("/refresh-token",
        async (RefreshRequestDto refresh, [FromServices] IAuthService _service) =>
        {
            return Results.Ok(await _service.RefreshTokenAsync(refresh));
        })
        .WithName("RefreshToken");


        userGroup.MapDelete("/log-out",
        async (string refreshToken, [FromServices] IAuthService _service) =>
        {
            await _service.LogOut(refreshToken);
            return Results.Ok();
        })
        .WithName("LogOut");
    }
}

