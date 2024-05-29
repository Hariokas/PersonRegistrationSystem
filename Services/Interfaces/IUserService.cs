using Repository.Models;
using Shared.DTOs;

namespace Services.Interfaces;

public interface IUserService
{
    Task AddUserAsync(User user);
    Task DeleteUserAsync(User user);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> GetUserByNameAsync(string name);
}