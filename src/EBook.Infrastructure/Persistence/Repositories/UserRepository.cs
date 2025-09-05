using EBook.Application.Dtos.Pagination;
using EBook.Application.Interfaces;
using EBook.Domain.Entities;
using EBook.Errors;
using Microsoft.EntityFrameworkCore;

namespace EBook.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext _appDbContext) : IUserRepository
{
    public async Task UpdateUserRoleAsync(long userId, string userRole)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user is null)
            throw new ArgumentNullException($"User not exists with this ID: {userId} (Repository)");

        var role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == userRole);
        if (role == null)
            throw new EntityNotFoundException($"UserId : {userId} -- Role : {userRole} not found");

        user.RoleId = role.RoleId;
        _appDbContext.Users.Update(user);

        await _appDbContext.SaveChangesAsync();
    }

    public async Task RemoveUserAsync(long userId, string userRole)
    {
        if (userRole == "SuperAdmin")
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user is null)
                throw new ArgumentNullException($"User not exists with this ID: {user} (Repository)");
            _appDbContext.Remove(user);
        }

        else if (userRole == "Admin")
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user is null)
                throw new ArgumentNullException($"User not exists with this ID: {user} (Repository)");

            if (user.Role.RoleName == "User")
            {
                _appDbContext.Remove(user);
            }
            else
            {
                throw new NotAllowedException($"UserId : {userId} -- Admin can not delete Admin or SuperAdmin");
            }
        }
    }

    public async Task<ICollection<User>> SelectAllUsersAsync(PageModel? pageModel)
    {
        IQueryable<User> users;
        if (pageModel is not null)
        {
            users = _appDbContext.Users
           .AsNoTracking()
           .Include(u => u.Role)
           .OrderBy(u => u.UserName)
           .Skip(pageModel.Skip)
           .Take(pageModel.Take);
        }
        else
        {
            users = _appDbContext.Users.Include(_ => _.Role).AsNoTracking().AsQueryable();
        }


        var query = users.ToQueryString();
        //_logger.LogInformation("Database query: " + query);

        return await users.ToListAsync();
    }

    public async Task<ICollection<User>> SelectAllUsersByRoleAsync(string role)
    {
        var users = await _appDbContext.Users.Include(_ => _.Role).AsNoTracking().Where(u => u.Role.RoleName == role).ToListAsync();
        //if (users is null || users.Count == 0)
            //throw new NotFoundException($"No users found with role: {role}");

        return users;
    }

    public async Task<User> SelectUserByIdAsync(long userId)
    {
        var user = await _appDbContext.Users.Include(_ => _.Role).FirstOrDefaultAsync(u => u.UserId == userId);
        if (user is null)
            throw new ArgumentNullException($"User not exists with this ID: {user} (Repository)");

        return user;
    }

    public async Task<User> SelectUserByUserNameAsync(string userName)
    {
        var user = await _appDbContext.Users.Include(_ => _.Role).Include(u => u.Confirmer).FirstOrDefaultAsync(u => u.UserName == userName);
        if (user is null)
            throw new ArgumentNullException($"User not exists with this UserName: {user} (Repository)");

        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        var existingUser = await _appDbContext.Users.FindAsync(user.UserId);
        if (existingUser != null)
        {
            _appDbContext.Entry(existingUser).State = EntityState.Detached;
        }

        _appDbContext.Users.Update(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<int> SelectTotalUsersCountByRoleAsync(string role)
    {
        var usersCount = await _appDbContext.Users.Include(u => u.Role)
            .Where(u => u.Role.RoleName == role).CountAsync();

        if (usersCount is 0)
            throw new NotFoundException($"No users found with role: {role}");

        return usersCount;
    }

    public async Task<bool> UserExistsAsync(long userId)
    {
        return await _appDbContext.Users.AnyAsync(u => u.UserId == userId);
    }

    public async Task<long> InsertUserAsync(User user)
    {
        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
        return user.UserId;
    }

    public async Task InsertConfirmer(UserConfirmer confirmer) // Default email qoshiladi, va shu email orqali random code boradi
    {
        await _appDbContext.UserConfirmers.AddAsync(confirmer);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<User> SelectUserByEmail(string email)
    {
        var user = await _appDbContext.Users.Include(_ => _.Confirmer).Include(u => u.Confirmer).FirstOrDefaultAsync(x => x.Confirmer.Gmail == email);

        if (user is null)
        {
            throw new EntityNotFoundException();
        }
        return user;
    }

    public async Task RemoveUserByIdAsync(long userId)
    {
        var user = await SelectUserByIdAsync(userId);
        _appDbContext.Users.Remove(user);
        await _appDbContext.SaveChangesAsync();
    }

    public Task<bool> CheckUserById(long userId) => _appDbContext.Users.AnyAsync(x => x.UserId == userId);

    public Task<bool> CheckUsernameExists(string username) => _appDbContext.Users.AnyAsync(_ => _.UserName == username);

    public async Task<long?> CheckEmailExistsAsync(string email)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(_ => _.Confirmer.Gmail == email);
        if (user is null)
        {
            return null;
        }
        return user.UserId;
    }

    public Task<bool> CheckPhoneNumberExists(string phoneNum) => _appDbContext.Users.AnyAsync(_ => _.PhoneNumber == phoneNum);
}
