using EBook.Application.Dtos;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/auth")
            .WithTags("Auth Management");
        //forgot password
        userGroup.MapPost("/forgot-password",
        async (string email,string newPassword, string confirmCode, IAuthService _service) =>
        {
            await _service.ForgotPassword(email,newPassword,confirmCode);
        })
        .WithName("ForgotPassword");

        // ---------- SignUp ----------
        userGroup.MapPost("/signup",
        async (UserCreateDto userDto, [FromServices] IAuthService authService) =>
        {
            var result = await authService.SignUpUserAsync(userDto);
            return Results.Created($"/api/auth/users/{result}", result);
        })
        .WithName("SignUp");

        // ---------- Send verification code ----------
        userGroup.MapPost("/send-code",
            async (string email, [FromServices] IAuthService _service) =>
            {
                await _service.EailCodeSender(email);
                return Results.Accepted();
            })
        .WithName("SendCode");

        // ---------- Confirm verification code ----------
        userGroup.MapPost("/confirm-code",
            async (string code, string email, [FromServices] IAuthService _service) =>
            {
                var res = await _service.ConfirmCode(code, email);
                return Results.Ok(res);
            })
        .WithName("ConfirmCode");

        // ---------- Login ----------
        userGroup.MapPost("/login",
            async (LoginDto userLoginDto, [FromServices] IAuthService authService) =>
            {
                var token = await authService.LoginUserAsync(userLoginDto);
                return Results.Ok(token);
            })
        .WithName("Login");

        // ---------- Refresh Token ----------
        userGroup.MapPost("/refresh-token",
        async (RefreshRequestDto refresh, [FromServices] IAuthService _service) =>
        {
            var newTokens = await _service.RefreshTokenAsync(refresh);
            return Results.Ok(newTokens);
        })
        .WithName("RefreshToken");

        // ---------- Logout ----------
        userGroup.MapDelete("/logout",
        async (string refreshToken, [FromServices] IAuthService _service) =>
        {
            await _service.LogOut(refreshToken);
            return Results.NoContent();
        })
        .WithName("LogOut");
    }
}
