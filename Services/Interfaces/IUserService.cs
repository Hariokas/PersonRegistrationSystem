using Shared.DTOs.User;
using Shared.Enums;

namespace Services.Interfaces;

public interface IUserService
{
    Task<UserDto> AddUserAsync(UserAuthenticateDto user);
    Task<UserDto> AuthenticateUserAsync(UserAuthenticateDto user);
    Task<AdminUserDto> DeleteUserAsync(AdminUserDto user);
    Task<AdminUserDto> DeleteUserAsAdminAsync(AdminUserDto user);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> GetUserByNameAsync(string name);
    Task<Guid> GetUserIdByNameAsync(string name);
    Task<AdminUserDto> GetUserAsAdminByIdAsync(Guid id);
    Task<AdminUserDto> GetUserAsAdminByNameAsync(string name);
    Task<Role> GetUserRoleByIdAsync(Guid id);
}