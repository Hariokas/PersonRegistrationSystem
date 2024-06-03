using Shared.DTOs;
using Shared.Enums;

namespace Services.Interfaces;

public interface IUserService
{
    Task<UserDto> AddUserAsync(UserAuthenticateDto user);
    Task<UserDto> AuthenticateUserAsync(UserAuthenticateDto user);
    Task<AdminUserDto> DeleteUserAsync(AdminUserDto user);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> GetUserByNameAsync(string name);
    Task<Guid> GetUserIdByNameAsync(string name);
    Task<Role> GetUserRoleByIdAsync(Guid id);
}