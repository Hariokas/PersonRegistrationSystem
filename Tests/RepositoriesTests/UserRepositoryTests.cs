using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Database;
using Repository.Models;
using Shared.Enums;

namespace Tests.RepositoriesTests;

[TestClass]
public class UserRepositoryTests
{
    private PersonRegistrationContext _dbContext;
    private UserRepository _userRepository;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<PersonRegistrationContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _dbContext = new PersonRegistrationContext(options);
        _userRepository = new UserRepository(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task AddUserAsync_ShouldAddUser()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            Password = "hashedpassword",
            Salt = "randomsalt",
            Role = Role.User
        };

        // Act
        await _userRepository.AddUserAsync(user);

        // Assert
        var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.IsNotNull(dbUser);
        Assert.AreEqual(user.Username, dbUser.Username);
    }

    [TestMethod]
    public async Task DeleteUserAsync_ShouldDeleteUser()
    {
        // Arrange
        var user = new User
        {
            Username = "deleteuser",
            Password = "hashedpassword",
            Salt = "randomsalt",
            Role = Role.User
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        await _userRepository.DeleteUserAsync(user);

        // Assert
        var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.IsNull(dbUser);
    }

    [TestMethod]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User
        {
            Username = "getuserbyid",
            Password = "hashedpassword",
            Salt = "randomsalt",
            Role = Role.User
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var dbUser = await _userRepository.GetUserByIdAsync(user.Id);

        // Assert
        Assert.IsNotNull(dbUser);
        Assert.AreEqual(user.Username, dbUser.Username);
    }

    [TestMethod]
    public async Task GetUserByNameAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User
        {
            Username = "getuserbyname",
            Password = "hashedpassword",
            Salt = "randomsalt",
            Role = Role.User
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var dbUser = await _userRepository.GetUserByNameAsync(user.Username);

        // Assert
        Assert.IsNotNull(dbUser);
        Assert.AreEqual(user.Username, dbUser.Username);
    }
}