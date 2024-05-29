using Repository.Interfaces;
using Repository.Models;

namespace Repository;

public class UserRepository : IUserRepository
{
    public Task AddUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByNameAsync(string name)
    {
        throw new NotImplementedException();
    }
}