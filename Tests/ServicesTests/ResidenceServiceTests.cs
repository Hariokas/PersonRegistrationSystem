using Moq;
using Repository.Interfaces;
using Repository.Models;
using Services;
using Shared;
using Shared.DTOs.Residence;
using Shared.Enums;

namespace Tests.ServicesTests;

[TestClass]
public class ResidenceServiceTests
{
    private Mock<IPersonRepository> _personRepositoryMock;
    private Mock<IResidenceRepository> _residenceRepositoryMock;
    private ResidenceService _residenceService;

    [TestInitialize]
    public void Setup()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _residenceRepositoryMock = new Mock<IResidenceRepository>();
        _residenceService = new ResidenceService(_personRepositoryMock.Object, _residenceRepositoryMock.Object);
    }

    [TestMethod]
    public async Task AddResidenceAsync_ShouldThrowPersonNotFoundException_WhenPersonNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var residenceCreateDto = new ResidenceCreateDto { PersonId = Guid.NewGuid() };
        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(residenceCreateDto.PersonId)).ReturnsAsync((Person)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<PersonNotFoundException>(() =>
            _residenceService.AddResidenceAsync(userId, residenceCreateDto));
    }

    [TestMethod]
    public async Task AddResidenceAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotAuthorized()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var person = new Person { User = new User() };
        var residenceCreateDto = new ResidenceCreateDto { PersonId = person.Id };

        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(residenceCreateDto.PersonId)).ReturnsAsync(person);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
            _residenceService.AddResidenceAsync(userId, residenceCreateDto));
    }

    [TestMethod]
    public async Task AddResidenceAsync_ShouldAddResidence_WhenValid()
    {
        // Arrange

        var person = new Person { User = new User() };
        var residenceCreateDto = new ResidenceCreateDto { PersonId = person.Id };
        var userId = person.UserId;
        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(residenceCreateDto.PersonId)).ReturnsAsync(person);

        // Act
        await _residenceService.AddResidenceAsync(userId, residenceCreateDto);

        // Assert
        _residenceRepositoryMock.Verify(x => x.AddResidenceAsync(It.IsAny<Residence>()), Times.Once);
    }

    [TestMethod]
    public async Task GetResidenceByIdAsync_ShouldThrowResidenceNotFoundException_WhenResidenceNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var residenceId = Guid.NewGuid();
        _residenceRepositoryMock.Setup(x => x.GetResidenceByIdAsync(residenceId)).ReturnsAsync((Residence)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ResidenceNotFoundException>(() =>
            _residenceService.GetResidenceByIdAsync(userId, Role.User, residenceId));
    }

    [TestMethod]
    public async Task GetResidenceByIdAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotAuthorized()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var residence = new Residence { Person = new Person { User = new User() } };
        var residenceId = residence.Id;
        _residenceRepositoryMock.Setup(x => x.GetResidenceByIdAsync(residenceId)).ReturnsAsync(residence);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
            _residenceService.GetResidenceByIdAsync(userId, Role.User, residenceId));
    }

    [TestMethod]
    public async Task GetResidenceByIdAsync_ShouldReturnResidenceDto_WhenValid()
    {
        // Arrange
        var residence = new Residence { Person = new Person { User = new User() } };
        var residenceId = residence.Id;
        var userId = residence.Person.UserId;
        _residenceRepositoryMock.Setup(x => x.GetResidenceByIdAsync(residenceId)).ReturnsAsync(residence);

        // Act
        var result = await _residenceService.GetResidenceByIdAsync(userId, Role.User, residenceId);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetResidenceByPersonIdAsync_ShouldThrowPersonNotFoundException_WhenPersonNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var personId = Guid.NewGuid();
        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(personId)).ReturnsAsync((Person)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<PersonNotFoundException>(() =>
            _residenceService.GetResidenceByPersonIdAsync(userId, Role.User, personId));
    }

    [TestMethod]
    public async Task GetResidenceByPersonIdAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotAuthorized()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var person = new Person { User = new User() };
        var personId = person.Id;

        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(personId)).ReturnsAsync(person);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
            _residenceService.GetResidenceByPersonIdAsync(userId, Role.User, personId));
    }

    [TestMethod]
    public async Task GetResidenceByPersonIdAsync_ShouldReturnResidenceDto_WhenValid()
    {
        // Arrange
        var person = new Person { User = new User() };
        var residence = new Residence { PersonId = person.Id, Person = person };
        var userId = person.User.Id;
        var personId = person.Id;
        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(personId)).ReturnsAsync(person);
        _residenceRepositoryMock.Setup(x => x.GetResidenceByPersonIdAsync(personId)).ReturnsAsync(residence);

        // Act
        var result = await _residenceService.GetResidenceByPersonIdAsync(userId, Role.User, personId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(residence.Id, result.Id);
    }

    [TestMethod]
    public async Task UpdateResidenceAsync_ShouldThrowResidenceNotFoundException_WhenResidenceNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var residenceUpdateDto = new ResidenceUpdateDto { Id = Guid.NewGuid() };
        _residenceRepositoryMock.Setup(x => x.GetResidenceByIdAsync(residenceUpdateDto.Id))
            .ReturnsAsync((Residence)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ResidenceNotFoundException>(() =>
            _residenceService.UpdateResidenceAsync(userId, Role.User, residenceUpdateDto));
    }

    [TestMethod]
    public async Task UpdateResidenceAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotAuthorized()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var residence = new Residence { Person = new Person { User = new User() } };
        var residenceUpdateDto = new ResidenceUpdateDto { Id = residence.Id };
        _residenceRepositoryMock.Setup(x => x.GetResidenceByIdAsync(residenceUpdateDto.Id)).ReturnsAsync(residence);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
            _residenceService.UpdateResidenceAsync(userId, Role.User, residenceUpdateDto));
    }

    [TestMethod]
    public async Task UpdateResidenceAsync_ShouldUpdateResidence_WhenValid()
    {
        // Arrange
        var residence = new Residence { Person = new Person { User = new User() } };
        var residenceUpdateDto = new ResidenceUpdateDto { Id = residence.Id };
        var userId = residence.Person.UserId;
        _residenceRepositoryMock.Setup(x => x.GetResidenceByIdAsync(residenceUpdateDto.Id)).ReturnsAsync(residence);

        // Act
        await _residenceService.UpdateResidenceAsync(userId, Role.User, residenceUpdateDto);

        // Assert
        _residenceRepositoryMock.Verify(x => x.UpdateResidenceAsync(It.IsAny<Residence>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteResidenceAsync_ShouldThrowResidenceNotFoundException_WhenResidenceNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var residenceId = Guid.NewGuid();
        _residenceRepositoryMock.Setup(x => x.GetResidenceByIdAsync(residenceId)).ReturnsAsync((Residence)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ResidenceNotFoundException>(() =>
            _residenceService.DeleteResidenceAsync(userId, Role.User, residenceId));
    }

    [TestMethod]
    public async Task DeleteResidenceAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotAuthorized()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var residence = new Residence { Person = new Person { User = new User() } };
        var residenceId = residence.Id;
        _residenceRepositoryMock.Setup(x => x.GetResidenceByIdAsync(residenceId)).ReturnsAsync(residence);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
            _residenceService.DeleteResidenceAsync(userId, Role.User, residenceId));
    }

    [TestMethod]
    public async Task DeleteResidenceAsync_ShouldDeleteResidence_WhenAuthorizedAsAdmin()
    {
        // Arrange
        var residence = new Residence { Person = new Person { User = new User() } };
        var residenceId = residence.Id;
        var userId = residence.Person.UserId;
        _residenceRepositoryMock.Setup(x => x.GetResidenceByIdAsync(residenceId)).ReturnsAsync(residence);

        // Act
        await _residenceService.DeleteResidenceAsync(userId, Role.Admin, residenceId);

        // Assert
        _residenceRepositoryMock.Verify(x => x.DeleteResidenceAsync(It.IsAny<Residence>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteResidenceAsync_ShouldDeleteResidence_WhenUserIsOwner()
    {
        // Arrange
        var residence = new Residence { Person = new Person { User = new User() } };
        var residenceId = residence.Id;
        var userId = residence.Person.UserId;
        _residenceRepositoryMock.Setup(x => x.GetResidenceByIdAsync(residenceId)).ReturnsAsync(residence);

        // Act
        await _residenceService.DeleteResidenceAsync(userId, Role.User, residenceId);

        // Assert
        _residenceRepositoryMock.Verify(x => x.DeleteResidenceAsync(It.IsAny<Residence>()), Times.Once);
    }
}