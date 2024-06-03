using Shared.Enums;

namespace Repository.Models;

public class Person
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; } = Gender.Unknown;
    public string DateOfBirth { get; set; } = string.Empty;
    public string PersonalCode { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    // Profile Picture
    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = default!;
}