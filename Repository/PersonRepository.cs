using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Interfaces;
using Repository.Models;

namespace Repository;

public class PersonRepository(PersonRegistrationContext dbContext) : IPersonRepository
{
    public async Task AddPersonAsync(Person person)
    {
        await dbContext.People.AddAsync(person);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Person?> GetPersonByIdAsync(Guid id)
    {
        return await dbContext.People.Include(p => p.Residence)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Person>> GetPeopleByUserIdAsync(Guid userId)
    {
        return await dbContext.People.Include(p => p.Residence)
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task UpdatePersonAsync(Person person)
    {
        dbContext.People.Update(person);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeletePersonAsync(Person person)
    {
        if (File.Exists(person.ProfilePicturePath))
            File.Delete(person.ProfilePicturePath);

        dbContext.People.Remove(person);
        await dbContext.SaveChangesAsync();
    }
}