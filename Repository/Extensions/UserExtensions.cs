using Repository.Models;
using Shared.DTOs;

namespace Repository.Extensions;

public static class UserExtensions
{
    public static AdminUserDto ToAdminUserDto(this User user)
    {
        return new AdminUserDto
        {
            Id = user.Id,
            Username = user.Username
        };
    }

    public static UserDto ToUserDto(this User user)
    {
        return new UserDto
        {
            Username = user.Username
        };
    }

    public static UserAuthenticateDto ToUserAuthenticateDto(this User user)
    {
        return new UserAuthenticateDto
        {
            Username = user.Username,
            Password = user.Password
        };
    }
}