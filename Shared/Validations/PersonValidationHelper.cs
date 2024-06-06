using System.ComponentModel.DataAnnotations;
using Shared.DTOs.Person;

namespace Shared.Validations;

public static class PersonValidationHelper
{
    public static void ValidatePersonCreateDto(PersonCreateDto personCreateDto)
    {
        if (string.IsNullOrWhiteSpace(personCreateDto.FirstName))
            throw new ValidationException("First name is required.");

        if (string.IsNullOrWhiteSpace(personCreateDto.LastName))
            throw new ValidationException("Last name is required.");

        if (personCreateDto.Gender == null)
            throw new ValidationException("Gender is required.");

        if (string.IsNullOrWhiteSpace(personCreateDto.DateOfBirth))
            throw new ValidationException("Date of birth is required.");

        if (string.IsNullOrWhiteSpace(personCreateDto.PersonalCode))
            throw new ValidationException("Personal code is required.");

        if (string.IsNullOrWhiteSpace(personCreateDto.Phone))
            throw new ValidationException("Phone number is required.");

        if (string.IsNullOrWhiteSpace(personCreateDto.Email))
            throw new ValidationException("Email is required.");

        if (personCreateDto.ProfilePicturePath is null)
            throw new ValidationException("Profile picture is required.");
    }
}