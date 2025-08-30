using EBook.Application.Dtos;

namespace EBook.Application.Services.ServiceInterfaces;

public interface IRoleService
{
    Task<long> AddRoleAsync(RoleCreateDto role);
    Task DeleteRoleAsync(long roleId);
    Task<List<RoleGetDto>> GetAllRolesAsync();
    Task<RoleGetDto> GetRoleByIdAsync(long roleId);
    Task<long> GetRoleIdAsync(string role);
    Task ChangeRoleAsync(RoleGetDto roleUpdateDto); // For SuperAdmin to change role details
}
