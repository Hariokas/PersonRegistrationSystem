using Shared.Enums;

namespace Shared.DTOs;

public class PersonDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; } = Gender.Unknown;
    public string DateOfBirth { get; set; } = string.Empty;
    public string PersonalCode { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}