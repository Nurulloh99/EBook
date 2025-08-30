using System.Security.Claims;
using EBook.Application.Dtos;

namespace EBook.Application.Services.Helpers;

public interface ITokenService
{
    public string GenerateToken(UserGetDto user);
    public string GenerateRefreshToken();
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
