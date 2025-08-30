using EBook.Domain.Entities;

namespace EBook.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddRefreshToken(RefreshToken refreshToken);
    Task<RefreshToken> SelectRefreshToken(string refreshToken, long userId);
    Task DeleteRefreshToken(string refreshToken);
}
