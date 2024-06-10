using System.Security.Claims;
using API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;
using Shared.DTOs.User;
using Shared.Enums;

namespace Tests.ControllersTests;

[TestClass]
public class UserControllerTests
{
    private Mock<IUserService> _userServiceMock;
    private Mock<ITokenService> _tokenServiceMock;
    private UserController _userController;

    [TestInitialize]
    public void Setup()
    {
        _userServiceMock = new Mock<IUserService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _userController = new UserController(_userServiceMock.Object, _tokenServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "testuser")
        }, "mock"));

        _userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [TestMethod]
    public async Task RegisterUserAsync_ShouldReturnOk_WhenUserIsRegistered()
    {
        // Arrange
        var userDto = new UserAuthenticateDto();
        var newUserDto = new UserDto();
        _userServiceMock.Setup(service => service.AddUserAsync(userDto))
            .ReturnsAsync(newUserDto);

        // Act
        var result = await _userController.RegisterUserAsync(userDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(newUserDto, okResult.Value);
    }

    [TestMethod]
    public async Task AuthenticateUserAsync_ShouldReturnOk_WhenUserIsAuthenticated()
    {
        // Arrange
        var userDto = new UserAuthenticateDto();
        var existingUserDto = new UserDto { Username = "testuser" };
        var token = "testtoken";

        _userServiceMock.Setup(service => service.AuthenticateUserAsync(userDto))
            .ReturnsAsync(existingUserDto);
        _userServiceMock.Setup(service => service.GetUserIdByNameAsync(existingUserDto.Username))
            .ReturnsAsync(Guid.NewGuid());
        _tokenServiceMock.Setup(service => service.GenerateToken(It.IsAny<Guid>(), existingUserDto.Username))
            .Returns(token);

        // Act
        var result = await _userController.AuthenticateUserAsync(userDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(token, okResult.Value.GetType().GetProperty("Token")?.GetValue(okResult.Value));
    }

    [TestMethod]
    public async Task DeleteUserAsync_ShouldReturnOk_WhenUserIsDeleted()
    {
        // Arrange
        var userDto = new AdminUserDto { Username = "testuser" };
        var deletedUserDto = new AdminUserDto();
        _userServiceMock.Setup(service => service.GetUserRoleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Role.Admin);
        _userServiceMock.Setup(service => service.DeleteUserAsync(userDto))
            .ReturnsAsync(deletedUserDto);

        // Act
        var result = await _userController.DeleteUserAsync(userDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(deletedUserDto, okResult.Value);
    }

    [TestMethod]
    public async Task GetUserByIdAsync_ShouldReturnOk_WhenUserIsFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userDto = new UserDto();
        _userServiceMock.Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync(userDto);

        // Act
        var result = await _userController.GetUserByIdAsync(userId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(userDto, okResult.Value);
    }

    [TestMethod]
    public async Task GetUserByNameAsync_ShouldReturnOk_WhenUserIsFound()
    {
        // Arrange
        var userName = "testuser";
        var userDto = new UserDto();
        _userServiceMock.Setup(service => service.GetUserByNameAsync(userName))
            .ReturnsAsync(userDto);

        // Act
        var result = await _userController.GetUserByNameAsync(userName);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(userDto, okResult.Value);
    }
}