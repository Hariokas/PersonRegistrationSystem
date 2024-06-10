using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Residence;
using Shared.Enums;
using Shared.Validations;

namespace Shared.DTOs.Person;

public class PersonCreateDto
{
    [Required] 
    [StringLength(100, MinimumLength = 1)] 
    public string FirstName { get; set; } = "";

    [Required] 
    [StringLength(100, MinimumLength = 1)] 
    public string LastName { get; set; } = "";

    [Required] 
    public Gender Gender { get; set; } = Gender.Unknown;

    [Required] 
    [StringLength(10)]
    [DataType(DataType.Date)]
    [DateOfBirthValidation]
    public string DateOfBirth { get; set; } = "";

    [Required]
    [PersonalCodeValidation]
    public string PersonalCode { get; set; } = "";

    [Required]
    [StringLength(20)]
    [PhoneNumberValidation]
    public string Phone { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = "";

    [Required] 
    [ProfilePictureValidation] 
    public IFormFile ProfilePicturePath { get; set; } = default!;

    [Required] 
    public ResidencePersonCreateDto Residence { get; set; } = new();
}