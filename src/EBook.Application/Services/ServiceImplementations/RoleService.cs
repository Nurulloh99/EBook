using EBook.Application.Dtos;
using EBook.Application.Interfaces;
using EBook.Application.Services.ServiceInterfaces;
using EBook.Domain.Entities;
using EBook.Errors;
using Microsoft.Extensions.Logging;

namespace EBook.Application.Services.ServiceImplementations;

public class RoleService(IRoleRepository _roleRepository, ILogger<UserService> _logger) : IRoleService
{
    public Task<long> AddRoleAsync(RoleCreateDto role)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role), "Role cannot be null. (Service)");

        var roleEntity = MapService.ConvertToRoleEntity(role);
        return _roleRepository.InsertRoleAsync(roleEntity);
    }

    public async Task ChangeRoleAsync(RoleGetDto roleUpdateDto)
    {
        var role = await _roleRepository.SelectRoleByIdAsync(roleUpdateDto.RoleId);

        if (role.RoleId != roleUpdateDto.RoleId)
            throw new ForbiddenException($"This role {roleUpdateDto.RoleName} is not depend on you!");

        if (role is null)
            throw new ArgumentNullException($"Role {roleUpdateDto} not found. (Service)");

        role.RoleName = roleUpdateDto.RoleName;
        role.RoleDescription = roleUpdateDto.RoleDescription;

        _logger.LogInformation($"Selected role has been updated successfully, {DateTime.Now}");

        await _roleRepository.UpdateRoleAsync(role);
    }

    public async Task DeleteRoleAsync(long roleId)
    {
        await _roleRepository.RemoveRoleAsync(roleId);
    }

    public async Task<List<RoleGetDto>> GetAllRolesAsync()
    {
        var allUsers = await _roleRepository.SelectAllRolesAsync();
        var items = allUsers.Select(MapService.ConvertToRoleDto).ToList();

        if (items is null || items.Count == 0)
            throw new ArgumentNullException("No roles found. (Service)");

        _logger.LogInformation($"All roles have been selected successfully, {DateTime.Now}");

        return items;
    }

    public async Task<RoleGetDto> GetRoleByIdAsync(long roleId)
    {
        var role = await _roleRepository.SelectRoleByIdAsync(roleId);
        if (role is null)
            throw new ArgumentNullException($"Role with ID {roleId} not found. (Service)");

        return MapService.ConvertToRoleDto(role);
    }

    public async Task<long> GetRoleIdAsync(string role)
    {
        var roleEntity = await _roleRepository.SelectRoleByRoleNameAsync(role);
        if (roleEntity is null)
            throw new ArgumentNullException($"Role with name {role} not found. (Service)");

        return roleEntity.RoleId;
    }
}
