using EBook.Domain.Entities;
using EBook.Application.Dtos.Pagination;

namespace EBook.Application.Interfaces;

public interface IUserRepository
{
    Task<long> InsertUserAsync(User user);
    Task<User> SelectUserByIdAsync(long userId);
    Task RemoveUserAsync(long userId, string userRole);
    Task<ICollection<User>> SelectAllUsersAsync(PageModel? pageModel);
    Task UpdateUserAsync(User user);
    Task<bool> UserExistsAsync(long userId);
    Task<ICollection<User>> SelectAllUsersByRoleAsync(string role);
    Task<User> SelectUserByUserNameAsync(string userName);
    Task UpdateUserRoleAsync(long userId, string userRole);
    Task<int> SelectTotalUsersCountByRoleAsync(string role);
    Task InsertConfirmer(UserConfirmer confirmer);
    Task<User> SelectUserByEmail(string email);
    Task RemoveUserByIdAsync(long userId);
    Task<bool> CheckUserById(long userId);
    Task<bool> CheckUsernameExists(string username);
    Task<long?> CheckEmailExistsAsync(string email);
    Task<bool> CheckPhoneNumberExists(string phoneNum);
}
