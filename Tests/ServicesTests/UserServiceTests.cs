using Moq;
using Repository.Interfaces;
using Repository.Models;
using Services;
using Shared.DTOs.User;
using Shared.Enums;
using Shared;

namespace Tests.ServicesTests;

[TestClass]
public class UserServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private UserService _userService;

    [TestInitialize]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [TestMethod]
    public async Task AddUserAsync_ShouldThrowUserAlreadyExistsException_WhenUserAlreadyExists()
    {
        // Arrange
        var userAuthenticateDto = new UserAuthenticateDto { Username = "testuser", Password = "Test@123" };
        var existingUser = new User { Username = "testuser" };

        _userRepositoryMock.Setup(x => x.GetUserByNameAsync(userAuthenticateDto.Username)).ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UserAlreadyExistsException>(() => _userService.AddUserAsync(userAuthenticateDto));
    }

    [TestMethod]
    public async Task AddUserAsync_ShouldAddUser_WhenValid()
    {
        // Arrange
        var userAuthenticateDto = new UserAuthenticateDto { Username = "newuser", Password = "Test@123" };
        User createdUser = null;

        _userRepositoryMock.Setup(x => x.GetUserByNameAsync(userAuthenticateDto.Username)).ReturnsAsync((User)null);
        _userRepositoryMock.Setup(x => x.AddUserAsync(It.IsAny<User>())).Callback<User>(u => createdUser = u).Returns(Task.CompletedTask);

        // Act
        var result = await _userService.AddUserAsync(userAuthenticateDto);

        // Assert
        _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Once);
        Assert.IsNotNull(createdUser);
        Assert.AreEqual(userAuthenticateDto.Username, createdUser.Username);
        Assert.IsTrue(!string.IsNullOrEmpty(createdUser.Password));
        Assert.IsTrue(!string.IsNullOrEmpty(createdUser.Salt));
        Assert.AreEqual(Role.User, createdUser.Role);
        Assert.AreEqual(userAuthenticateDto.Username, result.Username);
    }

    [TestMethod]
    public async Task AuthenticateUserAsync_ShouldThrowUserNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var userAuthenticateDto = new UserAuthenticateDto { Username = "testuser", Password = "Test@123" };

        _userRepositoryMock.Setup(x => x.GetUserByNameAsync(userAuthenticateDto.Username)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => _userService.AuthenticateUserAsync(userAuthenticateDto));
    }

    [TestMethod]
    public async Task AuthenticateUserAsync_ShouldThrowInvalidCredentialsException_WhenPasswordIsInvalid()
    {
        // Arrange
        var userAuthenticateDto = new UserAuthenticateDto { Username = "testuser", Password = "WrongPassword" };
        var salt = UserService.GenerateSalt();
        var hashedPassword = UserService.HashPassword("CorrectPassword", salt);
        var dbUser = new User { Username = "testuser", Password = hashedPassword, Salt = salt, Role = Role.User };

        _userRepositoryMock.Setup(x => x.GetUserByNameAsync(userAuthenticateDto.Username)).ReturnsAsync(dbUser);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidCredentialsException>(() => _userService.AuthenticateUserAsync(userAuthenticateDto));
    }


    [TestMethod]
    public async Task AuthenticateUserAsync_ShouldReturnUser_WhenCredentialsAreValid()
    {
        // Arrange
        var userAuthenticateDto = new UserAuthenticateDto { Username = "testuser", Password = "Test@123" };
        var salt = UserService.GenerateSalt();
        var hashedPassword = UserService.HashPassword(userAuthenticateDto.Password, salt);
        var dbUser = new User { Username = "testuser", Password = hashedPassword, Salt = salt, Role = Role.User };

        _userRepositoryMock.Setup(x => x.GetUserByNameAsync(userAuthenticateDto.Username)).ReturnsAsync(dbUser);

        // Act
        var result = await _userService.AuthenticateUserAsync(userAuthenticateDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(dbUser.Username, result.Username);
    }

    [TestMethod]
    public async Task DeleteUserAsync_ShouldThrowInvalidCredentialsException_WhenUserIsNull()
    {
        // Arrange
        AdminUserDto user = null;

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidCredentialsException>(() => _userService.DeleteUserAsync(user));
    }

    [TestMethod]
    public async Task DeleteUserAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var user = new AdminUserDto { Id = Guid.NewGuid(), Username = "nonexistentuser" };

        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(user.Id)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => _userService.DeleteUserAsync(user));
    }

    [TestMethod]
    public async Task DeleteUserAsync_ShouldDeleteUser_WhenValid()
    {
        // Arrange
        var user = new AdminUserDto { Id = Guid.NewGuid(), Username = "testuser" };
        var dbUser = new User { Username = user.Username, Role = Role.User };

        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(user.Id)).ReturnsAsync(dbUser);

        // Act
        var result = await _userService.DeleteUserAsync(user);

        // Assert
        _userRepositoryMock.Verify(x => x.DeleteUserAsync(It.IsAny<User>()), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(dbUser.Username, result.Username);
    }

    [TestMethod]
    public async Task GetUserByIdAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => _userService.GetUserByIdAsync(userId));
    }

    [TestMethod]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dbUser = new User { Username = "testuser", Role = Role.User };

        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(dbUser);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(dbUser.Username, result.Username);
    }

    [TestMethod]
    public async Task GetUserByNameAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var username = "nonexistentuser";

        _userRepositoryMock.Setup(x => x.GetUserByNameAsync(username)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => _userService.GetUserByNameAsync(username));
    }

    [TestMethod]
    public async Task GetUserByNameAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var username = "testuser";
        var dbUser = new User { Username = username, Role = Role.User };

        _userRepositoryMock.Setup(x => x.GetUserByNameAsync(username)).ReturnsAsync(dbUser);

        // Act
        var result = await _userService.GetUserByNameAsync(username);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(dbUser.Username, result.Username);
    }

}