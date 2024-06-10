using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Database;
using Repository.Models;

namespace Tests.RepositoriesTests;

[TestClass]
public class ResidenceRepositoryTests
{
    private PersonRegistrationContext _dbContext;
    private ResidenceRepository _residenceRepository;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<PersonRegistrationContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _dbContext = new PersonRegistrationContext(options);
        _residenceRepository = new ResidenceRepository(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task AddResidenceAsync_ShouldAddResidence()
    {
        // Arrange
        var residence = new Residence
        {
            PersonId = Guid.NewGuid(),
            City = "Test City",
            Street = "Test Street",
            HouseNumber = "123"
        };

        // Act
        await _residenceRepository.AddResidenceAsync(residence);

        // Assert
        var dbResidence = await _dbContext.Residences.FirstOrDefaultAsync(r => r.Id == residence.Id);
        Assert.IsNotNull(dbResidence);
        Assert.AreEqual(residence.City, dbResidence.City);
        Assert.AreEqual(residence.Street, dbResidence.Street);
        Assert.AreEqual(residence.HouseNumber, dbResidence.HouseNumber);
    }

    [TestMethod]
    public async Task GetResidenceByIdAsync_ShouldReturnResidence_WhenResidenceExists()
    {
        // Arrange
        var person = new Person { FirstName = "John", LastName = "Doe" };
        var residence = new Residence
        {
            PersonId = person.Id,
            City = "Test City",
            Street = "Test Street",
            HouseNumber = "123",
            Person = person
        };
        _dbContext.People.Add(person);
        _dbContext.Residences.Add(residence);
        await _dbContext.SaveChangesAsync();

        // Act
        var dbResidence = await _residenceRepository.GetResidenceByIdAsync(residence.Id);

        // Assert
        Assert.IsNotNull(dbResidence);
        Assert.AreEqual(residence.City, dbResidence.City);
        Assert.AreEqual(residence.Street, dbResidence.Street);
        Assert.AreEqual(residence.HouseNumber, dbResidence.HouseNumber);
        Assert.AreEqual(person.FirstName, dbResidence.Person.FirstName);
    }

    [TestMethod]
    public async Task GetResidenceByPersonIdAsync_ShouldReturnResidence_WhenResidenceExists()
    {
        // Arrange
        var person = new Person { FirstName = "Jane", LastName = "Doe" };
        var residence = new Residence
        {
            PersonId = person.Id,
            City = "City1",
            Street = "Street1",
            HouseNumber = "123",
            Person = person
        };
        _dbContext.People.Add(person);
        _dbContext.Residences.Add(residence);
        await _dbContext.SaveChangesAsync();

        // Act
        var dbResidence = await _residenceRepository.GetResidenceByPersonIdAsync(person.Id);

        // Assert
        Assert.IsNotNull(dbResidence);
        Assert.AreEqual(residence.City, dbResidence.City);
        Assert.AreEqual(residence.Street, dbResidence.Street);
        Assert.AreEqual(residence.HouseNumber, dbResidence.HouseNumber);
    }


    [TestMethod]
    public async Task UpdateResidenceAsync_ShouldUpdateResidence()
    {
        // Arrange
        var residence = new Residence
        {
            PersonId = Guid.NewGuid(),
            City = "Old City",
            Street = "Old Street",
            HouseNumber = "111"
        };
        _dbContext.Residences.Add(residence);
        await _dbContext.SaveChangesAsync();

        // Act
        residence.City = "New City";
        residence.Street = "New Street";
        residence.HouseNumber = "222";
        await _residenceRepository.UpdateResidenceAsync(residence);

        // Assert
        var dbResidence = await _dbContext.Residences.FirstOrDefaultAsync(r => r.Id == residence.Id);
        Assert.IsNotNull(dbResidence);
        Assert.AreEqual("New City", dbResidence.City);
        Assert.AreEqual("New Street", dbResidence.Street);
        Assert.AreEqual("222", dbResidence.HouseNumber);
    }

    [TestMethod]
    public async Task DeleteResidenceAsync_ShouldDeleteResidence()
    {
        // Arrange
        var residence = new Residence
        {
            PersonId = Guid.NewGuid(),
            City = "Delete City",
            Street = "Delete Street",
            HouseNumber = "333"
        };
        _dbContext.Residences.Add(residence);
        await _dbContext.SaveChangesAsync();

        // Act
        await _residenceRepository.DeleteResidenceAsync(residence);

        // Assert
        var dbResidence = await _dbContext.Residences.FirstOrDefaultAsync(r => r.Id == residence.Id);
        Assert.IsNull(dbResidence);
    }
}