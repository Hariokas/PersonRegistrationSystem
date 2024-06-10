using Repository.Models;
using Shared.DTOs.Person;
using Shared.StaticHelpers;

namespace Repository.Extensions;

public static class PersonExtensions
{
    public static Person ToPerson(this PersonCreateDto personCreateDto, User userInfo, string imagePath)
    {
        var person = new Person
        {
            User = userInfo,
            UserId = userInfo.Id,
            DateOfBirth = personCreateDto.DateOfBirth,
            Email = personCreateDto.Email,
            Gender = personCreateDto.Gender,
            FirstName = personCreateDto.FirstName,
            LastName = personCreateDto.LastName,
            PersonalCode = personCreateDto.PersonalCode,
            Phone = personCreateDto.Phone,
            ProfilePicturePath = imagePath,
            Residence = new Residence
            {
                City = personCreateDto.Residence.City,
                HouseNumber = personCreateDto.Residence.HouseNumber,
                Street = personCreateDto.Residence.Street,
                ApartmentNumber = personCreateDto.Residence.ApartmentNumber
            }
        };

        person.Residence.Person = person;
        person.Residence.PersonId = person.Id;

        return person;
    }

    public static PersonDto ToPersonDto(this Person person)
    {
        return new PersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            Email = person.Email,
            PersonalCode = person.PersonalCode,
            Phone = person.Phone,
            ProfilePicturePath = person.ProfilePicturePath,
            Residence = person.Residence?.ToResidenceDto() ?? default!
        };
    }

    public static AdminPersonDto ToAdminPersonDto(this Person person)
    {
        return new AdminPersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
        };
    }

    public static void UpdateWithDto(this Person person, PersonUpdateDto personDto)
    {
        if (!string.IsNullOrWhiteSpace(personDto.FirstName))
            person.FirstName = personDto.FirstName;

        if (!string.IsNullOrWhiteSpace(personDto.LastName))
            person.LastName = personDto.LastName;

        if (personDto.Gender.HasValue)
            person.Gender = personDto.Gender.Value;

        if (!string.IsNullOrWhiteSpace(personDto.DateOfBirth))
            person.DateOfBirth = personDto.DateOfBirth;

        if (!string.IsNullOrWhiteSpace(personDto.PersonalCode))
            person.PersonalCode = personDto.PersonalCode;

        if (!string.IsNullOrWhiteSpace(personDto.Phone))
            person.Phone = personDto.Phone;

        if (!string.IsNullOrWhiteSpace(personDto.Email))
            person.Email = personDto.Email;
    }
}