using EBook.Application.Dtos;

namespace EBook.Application.Services.ServiceInterfaces;

public interface IAuthService
{
    Task LogOut(string token);
    Task<long> SignUpUserAsync(UserCreateDto userCreateDto);
    Task<LoginResponseDto> LoginUserAsync(LoginDto userLoginDto);
    Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto request);
    Task EailCodeSender(string email);
    Task<bool> ConfirmCode(string userCode, string email);
}
