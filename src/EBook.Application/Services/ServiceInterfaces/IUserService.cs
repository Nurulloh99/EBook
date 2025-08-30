using EBook.Application.Dtos;
using EBook.Application.Dtos.Pagination;

namespace EBook.Application.Services.ServiceInterfaces;

public interface IUserService
{
    Task<UserGetDto> GetUserByIdAsync(long userId);
    Task DeleteUserAsync(long userId, string userRole);
    Task<GetPageModel<UserGetDto>> GetAllUsersAsync(PageModel? pageModel = null);
    Task UpdateUserAsync(UserGetDto userUpdateDto);
    Task UpdateUserRoleAsync(long userId, string userRole);
    Task<ICollection<UserGetDto>> GetAllUsersByRoleAsync(string role);
    Task<UserGetDto> GetUserByUserNameAsync(string userName);
    Task<bool> UserExistsAsync(long userId);
    Task<int> GetTotalUsersCountByRoleAsync(string role);
    Task<ICollection<UserGetDto>> SearchUsersByKeywordAsync(string keyword);
}
