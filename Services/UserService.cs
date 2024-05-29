using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Repository.Interfaces;
using Repository.Models;
using Services.Interfaces;
using Shared;
using Shared.DTOs;

namespace Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public Task AddUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(User user)
    {
        if (user == null || user.Id == Guid.Empty || string.IsNullOrEmpty(user.Username))
            throw new InvalidCredentialsException("Invalid credentials or user does not exists");

        var dbUser = userRepository.GetUserByIdAsync(user.Id);
        if (dbUser == null)
            throw new UserNotFoundException("User not found");

        userRepository.DeleteUserAsync(user);

        return Task.CompletedTask;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        if (user == null)
            throw new UserNotFoundException("User not found");

        return ToDto(user);
    }

    public Task<UserDto> GetUserByNameAsync(string name)
    {
        throw new NotImplementedException();
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

    private static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Username = user.Username
        };
    }
}