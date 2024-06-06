using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Repository.Extensions;
using Repository.Interfaces;
using Repository.Models;
using Services.Interfaces;
using Shared;
using Shared.DTOs.User;
using Shared.Enums;
using static Shared.Validations.UserValidationHelper;

namespace Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserDto> AddUserAsync(UserAuthenticateDto user)
    {
        ValidateUser(user.Username, user.Password);

        var existingUser = await userRepository.GetUserByNameAsync(user.Username);
        if (existingUser != null)
            throw new UserAlreadyExistsException("User already exists");

        var salt = GenerateSalt();
        var hashedPassword = HashPassword(user.Password, salt);

        var newUser = new User
        {
            Username = user.Username,
            Password = hashedPassword,
            Salt = salt,
            People = [],
            Role = Role.User
        };

        await userRepository.AddUserAsync(newUser);
        return newUser.ToUserDto();
    }

    public async Task<UserDto> AuthenticateUserAsync(UserAuthenticateDto user)
    {
        if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            throw new InvalidCredentialsException("Invalid credentials");

        var dbUser = await userRepository.GetUserByNameAsync(user.Username);
        if (dbUser == null)
            throw new UserNotFoundException("User not found");

        var hashedPassword = HashPassword(user.Password, dbUser.Salt);
        if (hashedPassword != dbUser.Password)
            throw new InvalidCredentialsException("Invalid credentials");

        return dbUser.ToUserDto();
    }

    public async Task<AdminUserDto> DeleteUserAsync(AdminUserDto user)
    {
        if (user == null || user.Id == Guid.Empty || string.IsNullOrEmpty(user.Username))
            throw new InvalidCredentialsException("Invalid credentials or user does not exists");

        var dbUser = await userRepository.GetUserByIdAsync(user.Id);
        if (dbUser == null)
            throw new UserNotFoundException("User not found");

        await userRepository.DeleteUserAsync(dbUser);
        return dbUser.ToAdminUserDto();
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        if (user == null)
            throw new UserNotFoundException("User not found");

        return user.ToUserDto();
    }

    public async Task<UserDto> GetUserByNameAsync(string name)
    {
        var user = await userRepository.GetUserByNameAsync(name);
        if (user == null)
            throw new UserNotFoundException("User not found");

        return user.ToUserDto();
    }

    public async Task<Guid> GetUserIdByNameAsync(string name)
    {
        var user = await userRepository.GetUserByNameAsync(name);
        if (user == null)
            throw new UserNotFoundException("User not found");

        return user.Id;
    }

    public async Task<Role> GetUserRoleByIdAsync(Guid id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        if (user == null)
            throw new UserNotFoundException("User not found");

        return user.Role;
    }

    private static string GenerateSalt()
    {
        var salt = new byte[128 / 8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        return Convert.ToBase64String(salt);
    }

    private static string HashPassword(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password,
            saltBytes,
            KeyDerivationPrf.HMACSHA256,
            10000,
            256 / 8
        ));

        return hashed;
    }
}