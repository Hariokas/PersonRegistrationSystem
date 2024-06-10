using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Repository.Interfaces;
using Repository.Models;
using Services;
using Shared;
using Shared.DTOs.Person;
using Shared.Enums;

namespace Tests.ServicesTests;

[TestClass]
public class PersonServiceTests
{
    private IFixture _fixture;
    private Mock<IPersonRepository> _personRepositoryMock;
    private PersonService _personService;
    private Mock<IUserRepository> _userRepositoryMock;

    [TestInitialize]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _personRepositoryMock = _fixture.Freeze<Mock<IPersonRepository>>();
        _userRepositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
        _personService = new PersonService(_personRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [TestMethod]
    public async Task AddPerson_ShouldThrowUserNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var personDto = _fixture.Create<PersonCreateDto>();
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UserNotFoundException>(() =>
            _personService.AddPersonAsync(userId, personDto));
    }

    [TestMethod]
    public async Task GetPersonByIdAsync_ShouldReturnPersonDto_WhenPersonFoundAndAuthorized()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var personId = _fixture.Create<Guid>();
        var person = _fixture.Build<Person>().With(p => p.UserId, userId).Create();
        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(personId)).ReturnsAsync(person);

        // Act
        var result = await _personService.GetPersonByIdAsync(userId, Role.User, personId);

        // Assert
        Assert.IsNotNull(result);
        _personRepositoryMock.Verify(x => x.GetPersonByIdAsync(personId), Times.Once);
    }

    [TestMethod]
    public async Task GetPersonByIdAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotAuthorized()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var personId = _fixture.Create<Guid>();
        var person = _fixture.Create<Person>();
        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(personId)).ReturnsAsync(person);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
            _personService.GetPersonByIdAsync(userId, Role.User, personId));
    }

    [TestMethod]
    public async Task GetPeopleByUserIdAsync_ShouldReturnPersonDtoList()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var people = _fixture.Build<Person>().With(p => p.UserId, userId).CreateMany();
        _personRepositoryMock.Setup(x => x.GetPeopleByUserIdAsync(userId)).ReturnsAsync(people);

        // Act
        var result = await _personService.GetPeopleByUserIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(people.Count(), result.Count());
    }

    [TestMethod]
    public async Task UpdatePersonAsync_ShouldThrowPersonNotFoundException_WhenPersonNotFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var personUpdateDto = _fixture.Create<PersonUpdateDto>();
        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(personUpdateDto.Id)).ReturnsAsync((Person)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<PersonNotFoundException>(() =>
            _personService.UpdatePersonAsync(userId, Role.User, personUpdateDto));
    }

    [TestMethod]
    public async Task UpdatePersonPictureAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotAuthorized()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var personPictureUpdateDto = _fixture.Create<PersonPictureUpdateDto>();
        var person = _fixture.Create<Person>();
        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(personPictureUpdateDto.Id)).ReturnsAsync(person);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
            _personService.UpdatePersonPictureAsync(userId, personPictureUpdateDto));
    }

    [TestMethod]
    public async Task DeletePersonAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotAuthorized()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var personId = _fixture.Create<Guid>();
        var person = _fixture.Create<Person>();
        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(personId)).ReturnsAsync(person);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
            _personService.DeletePersonAsync(userId, Role.User, personId));
    }

    [TestMethod]
    public async Task DeletePersonPictureAsync_ShouldThrowPersonNotFoundException_WhenPersonNotFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var personId = _fixture.Create<Guid>();
        _personRepositoryMock.Setup(x => x.GetPersonByIdAsync(personId)).ReturnsAsync((Person)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<PersonNotFoundException>(() =>
            _personService.DeletePersonPictureAsync(userId, personId));
    }
}