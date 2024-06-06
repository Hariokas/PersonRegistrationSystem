using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Residence;
using Shared.Enums;

namespace Shared.DTOs.Person;

public class PersonCreateDto
{
    [Required] [StringLength(100)] 
    public string FirstName { get; set; } = default!;

    [Required] [StringLength(100)] 
    public string LastName { get; set; } = default!;

    [Required] 
    public Gender Gender { get; set; } = Gender.Unknown;

    [Required] [StringLength(10)] 
    public string DateOfBirth { get; set; } = default!;

    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Personal code must be 11 digits long")]
    public string PersonalCode { get; set; } = default!;

    [Required] [StringLength(20)] 
    public string Phone { get; set; } = default!;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = default!;

    [Required] 
    public IFormFile ProfilePicturePath { get; set; } = default!;

    [Required] 
    public ResidencePersonCreateDto Residence { get; set; } = default!;
}