using EBook.Application.Dtos;
using EBook.Application.Interfaces;
using EBook.Application.Services.Helpers;
using EBook.Application.Services.Helpers.Security;
using EBook.Application.Services.ServiceInterfaces;
using EBook.Core.Errors;
using EBook.Domain.Entities;
using EBook.Errors;
using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace EBook.Application.Services.ServiceImplementations;

public class AuthService(IUserRepository _userRepository,
    IRoleRepository _roleRepository,
    ITokenService _tokenService,
    IRefreshTokenRepository _refreshTokRepo) : IAuthService
{
    public async Task<bool> ConfirmCode(string userCode, string email)
    {
        var user = await _userRepository.SelectUserByEmail(email);
        var code = user.Confirmer.ConfirmingCode;
        if (code == null || code != userCode || user.Confirmer.ExpiredDate < DateTime.UtcNow || user.Confirmer.IsConfirmed is true)
        {
            throw new NotAllowedException("Code is incorrect");
        }
        user.Confirmer.IsConfirmed = true;
        await _userRepository.UpdateUserAsync(user);
        return true;
    }

    public async Task EailCodeSender(string email)
    {
        var user = await _userRepository.SelectUserByEmail(email);

        var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = 587,
            Credentials = new NetworkCredential("qahmadjon11@gmail.com", "nhksnhhxzdbbnqdw")
        });

        Email.DefaultSender = sender;

        var code = Random.Shared.Next(100000, 999999).ToString();

        var sendResponse = await Email
            .From("qahmadjon11@gmail.com")
            .To(email)
            .Subject("Your Confirming Code")
            .Body(code)
            .SendAsync();

        user.Confirmer!.ConfirmingCode = code;
        user.Confirmer.ExpiredDate = DateTime.Now.AddMinutes(10);
        await _userRepository.UpdateUserAsync(user);
    }
    public async Task ForgotPassword(string email,string newPassword, string confirmCode)
    {
        bool isValid = System.Text.RegularExpressions.Regex.IsMatch(email,@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");
        
        if (!isValid)
        {
            throw new NotAllowedException();
        }
        var user = await _userRepository.SelectUserByEmail(email);
        var code = user.Confirmer!.ConfirmingCode;
        if (code == null || code != confirmCode || user.Confirmer.ExpiredDate < DateTime.Now)
        {
            throw new Exception("Code is incorrect");
        }

        var taple = PasswordHasher.Hasher(newPassword);

        user.Password = taple.Hash;
        user.Salt = taple.Salt;

        await _userRepository.UpdateUserAsync(user);
    }
    public async Task<LoginResponseDto> LoginUserAsync(LoginDto userLoginDto)
    {
        var user = await _userRepository.SelectUserByUserNameAsync(userLoginDto.UserName);

        var checkUserPassword = PasswordHasher.Verify(userLoginDto.Password, user.Password, user.Salt);

        if (checkUserPassword == false || user.Confirmer.IsConfirmed is false)
        {
            throw new UnauthorizedException($"UserId -- {user.UserId} -- UserName or password incorrect or email not confirmed yet");
        }

        var userGetDto = MapService.ConvertToUserGetDto(user);


        var token = _tokenService.GenerateToken(userGetDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await _refreshTokRepo.AddRefreshToken(refreshTokenToDB);


        var loginResponseDto = new LoginResponseDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            UserData = userGetDto,
        };

        return loginResponseDto;
    }

    public async Task LogOut(string token) => await _refreshTokRepo.DeleteRefreshToken(token);

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        ClaimsPrincipal? principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) throw new ForbiddenException("Invalid access token.");


        var userClaim = principal.FindFirst(c => c.Type == "UserId");
        var userId = long.Parse(userClaim.Value);


        var refreshToken = await _refreshTokRepo.SelectRefreshToken(request.RefreshToken, userId);
        if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow || refreshToken.IsRevoked)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        refreshToken.IsRevoked = true;

        var user = await _userRepository.SelectUserByIdAsync(userId);

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RoleName = user.Role.RoleName,
        };

        var newAccessToken = _tokenService.GenerateToken(userGetDto);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await _refreshTokRepo.AddRefreshToken(refreshTokenToDB);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
        };
    }

    public async Task<long> SignUpUserAsync(UserCreateDto userCreateDto)
    {
        //var validatorResult = await _validator.ValidateAsync(userCreateDto);
        //if (!validatorResult.IsValid)
        //{
        //    string errorMessages = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage));
        //    throw new AuthException(errorMessages);
        //}

        User isEmailExists;
        try
        {
            isEmailExists = await _userRepository.SelectUserByEmail(userCreateDto.Email);
        }
        catch (Exception ex)
        {
            isEmailExists = null;
        }

        if (isEmailExists == null)
        {

            var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);

            var confirmer = new UserConfirmer()
            {
                IsConfirmed = false,
                Gmail = userCreateDto.Email,
            };


            var user = new User()
            {
                Confirmer = confirmer,
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                UserName = userCreateDto.UserName,
                PhoneNumber = userCreateDto.PhoneNumber,
                Password = tupleFromHasher.Hash,
                Email = userCreateDto.Email,
                Salt = tupleFromHasher.Salt,
                RoleId = await _roleRepository.SelectRoleIdAsync("User")
            };

            long userId = await _userRepository.InsertUserAsync(user);

            var foundUser = await _userRepository.SelectUserByIdAsync(userId);

            foundUser.Confirmer!.UserId = userId;

            await _userRepository.UpdateUserAsync(foundUser);

            return userId;
        }
        else if (isEmailExists.Confirmer!.IsConfirmed is false)
        {
            var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);

            var confirmer = new UserConfirmer()
            {
                IsConfirmed = false,
                Gmail = userCreateDto.Email,
            };

            isEmailExists.FirstName = userCreateDto.FirstName;
            isEmailExists.LastName = userCreateDto.LastName;
            isEmailExists.UserName = userCreateDto.UserName;
            isEmailExists.Email = userCreateDto.Email;

            isEmailExists.PhoneNumber = userCreateDto.PhoneNumber;
            isEmailExists.Password = tupleFromHasher.Hash;
            isEmailExists.Salt = tupleFromHasher.Salt;
            isEmailExists.RoleId = await _roleRepository.SelectRoleIdAsync("User");

            await _userRepository.UpdateUserAsync(isEmailExists);
            return isEmailExists.UserId;
        }

        throw new NotAllowedException("This email already confirmed");
    }
}
