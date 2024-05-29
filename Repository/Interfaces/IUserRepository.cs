using Repository.Models;

namespace Repository.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task DeleteUserAsync(User user);
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByNameAsync(string name);
}