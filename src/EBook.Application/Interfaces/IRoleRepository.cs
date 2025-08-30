using EBook.Domain.Entities;

namespace EBook.Application.Interfaces;

public interface IRoleRepository
{
    Task<Role> SelectRoleByIdAsync(long roleId);
    Task<long> InsertRoleAsync(Role role);
    Task RemoveRoleAsync(long roleId);
    Task<Role> SelectRoleByRoleNameAsync(string roleName);
    Task<List<Role>> SelectAllRolesAsync();
    Task UpdateRoleAsync(Role role);
    Task<long> SelectRoleIdAsync(string role);
}
