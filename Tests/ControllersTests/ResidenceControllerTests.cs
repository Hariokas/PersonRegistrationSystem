using System.Security.Claims;
using API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;
using Shared.DTOs.Residence;
using Shared.Enums;

namespace Tests.ControllersTests;

[TestClass]
public class ResidenceControllerTests
{
    private Mock<IResidenceService> _residenceServiceMock;
    private Mock<IUserService> _userServiceMock;
    private ResidenceController _residenceController;

    [TestInitialize]
    public void Setup()
    {
        _residenceServiceMock = new Mock<IResidenceService>();
        _userServiceMock = new Mock<IUserService>();
        _residenceController = new ResidenceController(_residenceServiceMock.Object, _userServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }, "mock"));

        _residenceController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [TestMethod]
    public async Task AddResidenceAsync_ShouldReturnOk_WhenResidenceIsAdded()
    {
        // Arrange
        var residenceDto = new ResidenceCreateDto();
        _residenceServiceMock.Setup(service => service.AddResidenceAsync(It.IsAny<Guid>(), residenceDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _residenceController.AddResidenceAsync(residenceDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task GetResidenceByIdAsync_ShouldReturnOk_WhenResidenceIsFound()
    {
        // Arrange
        var residenceId = Guid.NewGuid();
        var residenceDto = new ResidenceDto();
        _userServiceMock.Setup(service => service.GetUserRoleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Role.User);
        _residenceServiceMock.Setup(service => service.GetResidenceByIdAsync(It.IsAny<Guid>(), It.IsAny<Role>(), residenceId))
            .ReturnsAsync(residenceDto);

        // Act
        var result = await _residenceController.GetResidenceByIdAsync(residenceId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(residenceDto, okResult.Value);
    }

    [TestMethod]
    public async Task GetResidenceByPersonIdAsync_ShouldReturnOk_WhenResidenceIsFound()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var residenceDto = new ResidenceDto();
        _userServiceMock.Setup(service => service.GetUserRoleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Role.User);
        _residenceServiceMock.Setup(service => service.GetResidenceByPersonIdAsync(It.IsAny<Guid>(), It.IsAny<Role>(), personId))
            .ReturnsAsync(residenceDto);

        // Act
        var result = await _residenceController.GetResidenceByPersonIdAsync(personId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(residenceDto, okResult.Value);
    }

    [TestMethod]
    public async Task UpdateResidenceAsync_ShouldReturnOk_WhenResidenceIsUpdated()
    {
        // Arrange
        var residenceDto = new ResidenceUpdateDto();
        _userServiceMock.Setup(service => service.GetUserRoleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Role.User);
        _residenceServiceMock.Setup(service => service.UpdateResidenceAsync(It.IsAny<Guid>(), It.IsAny<Role>(), residenceDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _residenceController.UpdateResidenceAsync(residenceDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task DeleteResidenceAsync_ShouldReturnOk_WhenResidenceIsDeleted()
    {
        // Arrange
        var residenceId = Guid.NewGuid();
        _userServiceMock.Setup(service => service.GetUserRoleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Role.User);
        _residenceServiceMock.Setup(service => service.DeleteResidenceAsync(It.IsAny<Guid>(), It.IsAny<Role>(), residenceId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _residenceController.DeleteResidenceAsync(residenceId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
}