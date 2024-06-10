using Microsoft.AspNetCore.Http;
using Shared.DTOs.Person;
using Shared.Enums;

namespace Services.Interfaces;

public interface IPersonService
{
    Task AddPersonAsync(Guid userId, PersonCreateDto personDto);
    Task<PersonDto> GetPersonByIdAsync(Guid userId, Role userRole, Guid personId);
    Task<IEnumerable<PersonDto>> GetPeopleByUserIdAsync(Guid userId);
    Task<AdminPersonDto> GetPersonAsAdminByIdAsync(Guid personId);
    Task<MemoryStream> GetPictureByPersonId(Guid userId, Guid personId);
    Task UpdatePersonAsync(Guid currentUserId, Role currentUserRole, PersonUpdateDto personDto);
    Task UpdatePersonPictureAsync(Guid userId, PersonPictureUpdateDto personPictureUpdateDto);
    Task DeletePersonAsync(Guid currentUserId, Role currentUserRole, Guid personId);
    Task DeletePersonAsAdminAsync(Guid personId);
    Task DeletePersonPictureAsync(Guid userId, Guid personId);
}