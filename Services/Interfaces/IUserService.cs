using Shared.DTOs;

namespace Services.Interfaces;

public interface IUserService
{
    Task<UserDto> AddUserAsync(UserAuthenticateDto user);
    Task<UserDto> AuthenticateUserAsync(UserAuthenticateDto user);
    Task<AdminUserDto> DeleteUserAsync(AdminUserDto user);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> GetUserByNameAsync(string name);
}