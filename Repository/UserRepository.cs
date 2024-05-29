using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Interfaces;
using Repository.Models;

namespace Repository;

public class UserRepository(PersonRegistrationContext dbContext) : IUserRepository
{
    public async Task AddUserAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        await Task.Run(() => dbContext.Users.Remove(user));
        await dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByNameAsync(string name)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u =>
            u.Username.ToLower() == name.ToLower());
    }
}