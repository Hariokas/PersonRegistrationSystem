using System.ComponentModel.DataAnnotations;
using Shared.Validations;

namespace Shared.DTOs.Residence;

public class ResidenceCreateDto
{
    [Required]
    public Guid PersonId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    [NonEmptyString]
    public string City { get; set; } = default!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    [NonEmptyString]
    public string Street { get; set; } = default!;

    [Required]
    [StringLength(10, MinimumLength = 1)]
    [NonEmptyString]
    [RegularExpression(@"^\d+[a-zA-Z]*$", ErrorMessage = "Invalid house number format.")]
    public string HouseNumber { get; set; } = default!;

    [StringLength(10)]
    [RegularExpression(@"^\d+[a-zA-Z]*$", ErrorMessage = "Invalid house number format.")]
    public string? ApartmentNumber { get; set; }
}