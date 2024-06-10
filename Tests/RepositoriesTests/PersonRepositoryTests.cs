using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Database;
using Repository.Models;

namespace Tests.RepositoriesTests;

[TestClass]
public class PersonRepositoryTests
{
    private PersonRegistrationContext _dbContext;
    private PersonRepository _personRepository;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<PersonRegistrationContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new PersonRegistrationContext(options);
        _personRepository = new PersonRepository(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task AddPersonAsync_ShouldAddPerson()
    {
        // Arrange
        var person = new Person
        {
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        await _personRepository.AddPersonAsync(person);

        // Assert
        var dbPerson = await _dbContext.People.FirstOrDefaultAsync(p => p.Id == person.Id);
        Assert.IsNotNull(dbPerson);
        Assert.AreEqual(person.FirstName, dbPerson.FirstName);
    }

    [TestMethod]
    public async Task GetPersonByIdAsync_ShouldReturnPerson_WhenPersonExists()
    {
        // Arrange
        var person = new Person
        {
            UserId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Doe"
        };
        _dbContext.People.Add(person);
        await _dbContext.SaveChangesAsync();

        // Act
        var dbPerson = await _personRepository.GetPersonByIdAsync(person.Id);

        // Assert
        Assert.IsNotNull(dbPerson);
        Assert.AreEqual(person.FirstName, dbPerson.FirstName);
    }

    [TestMethod]
    public async Task GetPeopleByUserIdAsync_ShouldReturnPeople_WhenPeopleExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var people = new List<Person>
        {
            new() { UserId = userId, FirstName = "Alice", LastName = "Smith" },
            new() { UserId = userId, FirstName = "Bob", LastName = "Brown" }
        };
        _dbContext.People.AddRange(people);
        await _dbContext.SaveChangesAsync();

        // Act
        var dbPeople = (await _personRepository.GetPeopleByUserIdAsync(userId)).ToList();

        // Assert
        Assert.AreEqual(2, dbPeople.Count);
        Assert.IsTrue(dbPeople.Any(p => p.FirstName == "Alice"));
        Assert.IsTrue(dbPeople.Any(p => p.FirstName == "Bob"));
    }

    [TestMethod]
    public async Task UpdatePersonAsync_ShouldUpdatePerson()
    {
        // Arrange
        var person = new Person
        {
            UserId = Guid.NewGuid(),
            FirstName = "Charlie",
            LastName = "Clark"
        };
        _dbContext.People.Add(person);
        await _dbContext.SaveChangesAsync();

        // Act
        person.LastName = "Johnson";
        await _personRepository.UpdatePersonAsync(person);

        // Assert
        var dbPerson = await _dbContext.People.FirstOrDefaultAsync(p => p.Id == person.Id);
        Assert.IsNotNull(dbPerson);
        Assert.AreEqual("Johnson", dbPerson.LastName);
    }

    [TestMethod]
    public async Task DeletePersonAsync_ShouldDeletePerson()
    {
        // Arrange
        var person = new Person
        {
            UserId = Guid.NewGuid(),
            FirstName = "David",
            LastName = "Green"
        };
        _dbContext.People.Add(person);
        await _dbContext.SaveChangesAsync();

        // Act
        await _personRepository.DeletePersonAsync(person);

        // Assert
        var dbPerson = await _dbContext.People.FirstOrDefaultAsync(p => p.Id == person.Id);
        Assert.IsNull(dbPerson);
    }

}