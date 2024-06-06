using Repository.Models;

namespace Repository.Interfaces;

public interface IPersonRepository
{
    Task AddPersonAsync(Person person);
    Task<Person?> GetPersonByIdAsync(Guid id);
    Task<IEnumerable<Person>> GetPeopleByUserIdAsync(Guid userId);
    Task UpdatePersonAsync(Person person);
    Task DeletePersonAsync(Person person);
}