using Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Residence;

public class ResidenceUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [StringLength(100, MinimumLength = 2)]
    [NonEmptyString]
    public string? City { get; set; }

    [StringLength(100, MinimumLength = 2)]
    [NonEmptyString]
    public string? Street { get; set; }

    [StringLength(10, MinimumLength = 1)]
    [NonEmptyString]
    [RegularExpression(@"^\d+[a-zA-Z]*$", ErrorMessage = "Invalid house number format.")]
    public string? HouseNumber { get; set; }

    [StringLength(10)]
    [RegularExpression(@"^\d+[a-zA-Z]*$", ErrorMessage = "Invalid apartment number format.")]
    public string? ApartmentNumber { get; set; }
}