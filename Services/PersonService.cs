using Microsoft.AspNetCore.Http;
using Repository.Extensions;
using Repository.Interfaces;
using Services.Interfaces;
using Shared;
using Shared.DTOs.Person;
using Shared.Enums;
using Shared.StaticHelpers;
using static Shared.StaticHelpers.ImageHelper;
using static Shared.Validations.PersonValidationHelper;

namespace Services;

public class PersonService(IPersonRepository personRepository, IUserRepository userRepository)
    : IPersonService
{
    public async Task AddPersonAsync(Guid userId, PersonCreateDto personDto)
    {
        ValidatePersonCreateDto(personDto);

        var user = await userRepository.GetUserByIdAsync(userId) ??
                   throw new UserNotFoundException("User not found");


        var profilePicPath = await SaveImage(personDto.ProfilePicturePath);

        await personRepository.AddPersonAsync(personDto.ToPerson(user, profilePicPath));
    }

    public async Task<PersonDto> GetPersonByIdAsync(Guid userId, Role userRole, Guid personId)
    {
        var person = await personRepository.GetPersonByIdAsync(personId) ??
                     throw new PersonNotFoundException("Person not found");

        if (userId != person.UserId)
            throw new UnauthorizedAccessException("You are not authorized to view this person");

        return person.ToPersonDto();
    }

    public async Task<IEnumerable<PersonDto>> GetPeopleByUserIdAsync(Guid userId)
    {
        return (await personRepository.GetPeopleByUserIdAsync(userId))
            .Select(person => person.ToPersonDto());
    }

    public async Task<MemoryStream> GetPictureByPersonId(Guid userId, Guid personId)
    {
        var person = await personRepository.GetPersonByIdAsync(personId) ??
                     throw new PersonNotFoundException("Person not found");

        if (userId != person.UserId)
            throw new UnauthorizedAccessException("You are not authorized to view this person's picture");

        var imageMemoryStream = await ImageHelper.LoadImageAsync(person.ProfilePicturePath);

        return imageMemoryStream;
    }

    public async Task UpdatePersonAsync(Guid currentUserId, Role currentUserRole, PersonUpdateDto personUpdateDto)
    {
        var person = await personRepository.GetPersonByIdAsync(personUpdateDto.Id) ??
                     throw new PersonNotFoundException("Person not found");

        if (currentUserId != person.UserId)
            throw new UnauthorizedAccessException("You are not authorized to update this person");

        person.UpdateWithDto(personUpdateDto);

        await personRepository.UpdatePersonAsync(person);
    }

    public async Task UpdatePersonPictureAsync(Guid userId, PersonPictureUpdateDto personPictureUpdateDto)
    {
        var person = await personRepository.GetPersonByIdAsync(personPictureUpdateDto.Id) ??
                     throw new PersonNotFoundException("Person not found");

        if (userId != person.UserId)
            throw new UnauthorizedAccessException("You are not authorized to update this person's picture");

        var profilePicPath = await SaveImage(personPictureUpdateDto.ProfilePicture);

        if (File.Exists(person.ProfilePicturePath))
            File.Delete(person.ProfilePicturePath);

        person.ProfilePicturePath = profilePicPath;

        await personRepository.UpdatePersonAsync(person);
    }

    public async Task DeletePersonAsync(Guid currentUserId, Role currentUserRole, Guid personId)
    {
        var person = await personRepository.GetPersonByIdAsync(personId) ??
                     throw new PersonNotFoundException("Person not found");

        if (currentUserId != person.UserId && currentUserRole != Role.Admin)
            throw new UnauthorizedAccessException("You are not authorized to delete this person");

        if (File.Exists(person.ProfilePicturePath))
            File.Delete(person.ProfilePicturePath);

        await personRepository.DeletePersonAsync(person);
    }

    public async Task DeletePersonPictureAsync(Guid userId, Guid personId)
    {
        var person = await personRepository.GetPersonByIdAsync(personId) ??
                     throw new PersonNotFoundException("Person not found");

        if (userId != person.UserId)
            throw new UnauthorizedAccessException("You are not authorized to delete this person's picture");

        if (File.Exists(person.ProfilePicturePath))
            File.Delete(person.ProfilePicturePath);

        person.ProfilePicturePath = "";

        await personRepository.UpdatePersonAsync(person);
    }

}