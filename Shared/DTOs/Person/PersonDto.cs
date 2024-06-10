using Shared.DTOs.Residence;
using Shared.Enums;

namespace Shared.DTOs.Person;

public class PersonDto
{
    public Guid Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public Gender Gender { get; set; } = default!;
    public string DateOfBirth { get; set; } = default!;
    public string PersonalCode { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string ProfilePicturePath { get; set; } = default!;
    public ResidenceDto Residence { get; set; } = default!;
}