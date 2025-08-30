using EBook.Application.Interfaces;
using EBook.Domain.Entities;
using EBook.Errors;
using Microsoft.EntityFrameworkCore;

namespace EBook.Infrastructure.Persistence.Repositories;

public class RoleRepository(AppDbContext _appDbContext) : IRoleRepository
{
    public async Task<long> InsertRoleAsync(Role role)
    {
        await _appDbContext.Roles.AddAsync(role);
        await _appDbContext.SaveChangesAsync();
        return role.RoleId;
    }

    public async Task RemoveRoleAsync(long roleId)
    {
        var role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        if (role is null)
            throw new EntityNotFoundException($"Role with ID {roleId} not found.");

        _appDbContext.Roles.Remove(role);
        _appDbContext.SaveChanges();
    }

    public async Task<List<Role>> SelectAllRolesAsync()
    {
        var roles = await _appDbContext.Roles.AsNoTracking().ToListAsync();

        if (roles is null || roles.Count == 0)
            throw new EntityNotFoundException("No roles found. (Repository)");

        return roles;
    }

    public async Task<Role> SelectRoleByIdAsync(long roleId)
    {
        var role = await _appDbContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleId == roleId);
        if (role is null)
            throw new EntityNotFoundException($"Role with ID {roleId} not found. (Repository)");

        return role;
    }

    public async Task<Role> SelectRoleByRoleNameAsync(string roleName)
    {
        var role = await _appDbContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleName == roleName);
        if (role is null)
            throw new EntityNotFoundException($"Role with this RoleName: {roleName} not found.");

        return role;
    }

    public async Task<long> SelectRoleIdAsync(string role)
    {
        var foundRole = await _appDbContext.Roles.FirstOrDefaultAsync(_ => _.RoleName == role);
        if (foundRole is null)
        {
            throw new EntityNotFoundException(role + " - not found");
        }
        return foundRole.RoleId;
    }

    public async Task UpdateRoleAsync(Role role)
    {
        var existingRole = await _appDbContext.Roles.FindAsync(role.RoleId);
        if (existingRole != null)
        {
            _appDbContext.Entry(existingRole).State = EntityState.Detached;
        }

        _appDbContext.Roles.Update(role);
        await _appDbContext.SaveChangesAsync();
    }
}
