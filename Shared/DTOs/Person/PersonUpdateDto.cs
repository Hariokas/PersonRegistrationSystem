using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Person;

public class PersonUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    public Gender? Gender { get; set; }

    [StringLength(10)]
    public string? DateOfBirth { get; set; }

    public string? PersonalCode { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }
}