using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;
using EBook.Application.Interfaces;
using EBook.Application.Services.ServiceInterfaces;
using Microsoft.Extensions.Logging;

namespace EBook.Application.Services.ServiceImplementations;

public class UserService(IUserRepository _userRepository, ILogger<UserService> _logger) : IUserService
{
    public async Task DeleteUserAsync(long userId, string userRole)
    {
        await _userRepository.RemoveUserAsync(userId, userRole);
    }

    public async Task<GetPageModel<UserGetDto>> GetAllUsersAsync(PageModel pageModel)
    {
        var allUsers = await _userRepository.SelectAllUsersAsync(pageModel);
        var items = allUsers.Select(MapService.ConvertToUserGetDto).ToList();

        _logger.LogInformation($"Selected users have been taken successfully {DateTime.Now}");

        return new GetPageModel<UserGetDto>
        {
            PageModel = pageModel,
            TotalCount = items.Count,
            Items = items
        };
    }

    public async Task<ICollection<UserGetDto>> GetAllUsersByRoleAsync(string role)
    {
        var allOrders = await _userRepository.SelectAllUsersByRoleAsync(role);
        var result = allOrders.Select(MapService.ConvertToUserGetDto).ToList();

        _logger.LogInformation($"All users have been taken by role successfully {result}, {DateTime.Now}");

        return result;
    }

    public async Task<int> GetTotalUsersCountByRoleAsync(string role)
    {
        var totalCount = await _userRepository.SelectTotalUsersCountByRoleAsync(role);

        _logger.LogInformation($"Selected users count have been taken successfully {totalCount}, {DateTime.Now}");

        return totalCount;
    }

    public Task<UserGetDto> GetUserByIdAsync(long userId)
    {
        if (userId <= 0)
            throw new ArgumentNullException("User ID must be greater than zero.", nameof(userId));

        var user = _userRepository.SelectUserByIdAsync(userId);
        if (user == null)
            throw new ArgumentNullException($"User with id {userId} not found");

        _logger.LogInformation($"Selected user has been taken by ID successfully {user}, {DateTime.Now}");

        return Task.FromResult(MapService.ConvertToUserGetDto(user.Result));
    }

    public Task<UserGetDto> GetUserByUserNameAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentNullException("User name cannot be null or empty.", nameof(userName));

        var user = _userRepository.SelectUserByUserNameAsync(userName);
        if (user == null)
            throw new ArgumentNullException($"User with username {userName} not found");

        _logger.LogInformation($"Selected user has been taken by NAME successfully {user}, {DateTime.Now}");

        return Task.FromResult(MapService.ConvertToUserGetDto(user.Result));
    }

    public Task<ICollection<UserGetDto>> SearchUsersByKeywordAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            throw new ArgumentNullException("Keyword cannot be null or empty.", nameof(keyword));

        var users = _userRepository.SelectAllUsersAsync(new PageModel { Skip = 1, Take = 1000 }); // Adjust as needed
        var filteredUsers = users.Result.Where(u => u.UserName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
        || u.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)
        || u.FirstName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
        || u.LastName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
        || u.PhoneNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                        .Select(MapService.ConvertToUserGetDto).ToList();
        return Task.FromResult<ICollection<UserGetDto>>(filteredUsers);
    }

    public async Task UpdateUserAsync(UserGetDto userUpdateDto)
    {
        var user = await _userRepository.SelectUserByIdAsync(userUpdateDto.UserId);

        if (user == null)
            throw new ArgumentNullException($"User not found with id {userUpdateDto.UserId}");

        user.FirstName = userUpdateDto.FirstName;
        user.LastName = userUpdateDto.LastName;
        user.UserName = userUpdateDto.UserName;
        user.Email = userUpdateDto.Email;
        user.PhoneNumber = userUpdateDto.PhoneNumber;

        _logger.LogInformation($"Selected user has been updated successfully {user}, {DateTime.Now}");

        await _userRepository.UpdateUserAsync(user);
    }

    public async Task UpdateUserRoleAsync(long userId, string userRole)
    {
        await _userRepository.UpdateUserRoleAsync(userId, userRole);
    }


    public async Task<bool> UserExistsAsync(long userId)
    {
        return await _userRepository.UserExistsAsync(userId);
    }
}
