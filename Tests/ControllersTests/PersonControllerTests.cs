using System.Security.Claims;
using API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;
using Shared.DTOs.Person;
using Shared.Enums;

namespace Tests.ControllersTests;

[TestClass]
public class PersonControllerTests
{
    private PersonController _personController;
    private Mock<IPersonService> _personServiceMock;
    private Mock<IUserService> _userServiceMock;

    [TestInitialize]
    public void Setup()
    {
        _personServiceMock = new Mock<IPersonService>();
        _userServiceMock = new Mock<IUserService>();
        _personController = new PersonController(_personServiceMock.Object, _userServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }, "mock"));

        _personController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [TestMethod]
    public async Task AddPersonAsync_ShouldReturnOk_WhenPersonIsAdded()
    {
        // Arrange
        var personDto = new PersonCreateDto();
        _personServiceMock.Setup(service => service.AddPersonAsync(It.IsAny<Guid>(), personDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _personController.AddPersonAsync(personDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task GetPersonByIdAsync_ShouldReturnOk_WhenPersonIsFound()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var personDto = new PersonDto();
        _userServiceMock.Setup(service => service.GetUserRoleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Role.User);
        _personServiceMock.Setup(service => service.GetPersonByIdAsync(It.IsAny<Guid>(), It.IsAny<Role>(), personId))
            .ReturnsAsync(personDto);

        // Act
        var result = await _personController.GetPersonByIdAsync(personId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(personDto, okResult.Value);
    }

    [TestMethod]
    public async Task GetPeopleByUserIdAsync_ShouldReturnOk_WhenPeopleAreFound()
    {
        // Arrange
        var people = new List<PersonDto> { new(), new() };
        _userServiceMock.Setup(service => service.GetUserRoleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Role.User);
        _personServiceMock.Setup(service => service.GetPeopleByUserIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(people);

        // Act
        var result = await _personController.GetPeopleByUserIdAsync(null);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(people, okResult.Value);
    }

    [TestMethod]
    public async Task GetPersonPictureByIdAsync_ShouldReturnFile_WhenPictureIsFound()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var pictureStream = new MemoryStream();
        _personServiceMock.Setup(service => service.GetPictureByPersonId(It.IsAny<Guid>(), personId))
            .ReturnsAsync(pictureStream);

        // Act
        var result = await _personController.GetPersonPictureByIdAsync(personId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(FileStreamResult));
    }

    [TestMethod]
    public async Task UpdatePersonAsync_ShouldReturnOk_WhenPersonIsUpdated()
    {
        // Arrange
        var personUpdateDto = new PersonUpdateDto();
        _userServiceMock.Setup(service => service.GetUserRoleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Role.User);
        _personServiceMock.Setup(service =>
                service.UpdatePersonAsync(It.IsAny<Guid>(), It.IsAny<Role>(), personUpdateDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _personController.UpdatePersonAsync(personUpdateDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task UpdatePersonPictureAsync_ShouldReturnOk_WhenPictureIsUpdated()
    {
        // Arrange
        var personPictureUpdateDto = new PersonPictureUpdateDto();
        _personServiceMock.Setup(service => service.UpdatePersonPictureAsync(It.IsAny<Guid>(), personPictureUpdateDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _personController.UpdatePersonPictureAsync(personPictureUpdateDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task DeletePersonAsync_ShouldReturnOk_WhenPersonIsDeleted()
    {
        // Arrange
        var personId = Guid.NewGuid();
        _userServiceMock.Setup(service => service.GetUserRoleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Role.User);
        _personServiceMock.Setup(service => service.DeletePersonAsync(It.IsAny<Guid>(), It.IsAny<Role>(), personId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _personController.DeletePersonAsync(personId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task DeletePersonPictureAsync_ShouldReturnOk_WhenPictureIsDeleted()
    {
        // Arrange
        var personId = Guid.NewGuid();
        _personServiceMock.Setup(service => service.DeletePersonPictureAsync(It.IsAny<Guid>(), personId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _personController.DeletePersonPictureAsync(personId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task GetPersonAsAdminByIdAsync_ShouldReturnOk_WhenPersonIsFound()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var adminPersonDto = new AdminPersonDto { Id = personId, FirstName = "John", LastName = "Doe" };
        _personServiceMock.Setup(service => service.GetPersonAsAdminByIdAsync(personId))
            .ReturnsAsync(adminPersonDto);

        // Act
        var result = await _personController.GetPersonAsAdminByIdAsync(personId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(adminPersonDto, okResult.Value);
    }

    [TestMethod]
    public async Task DeletePersonAsAdminAsync_ShouldReturnOk_WhenPersonIsDeleted()
    {
        // Arrange
        var personId = Guid.NewGuid();
        _personServiceMock.Setup(service => service.DeletePersonAsAdminAsync(personId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _personController.DeletePersonAsAdminAsync(personId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
}