using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Residence;

public class ResidencePersonCreateDto
{
    [Required]
    [StringLength(100)]
    public string City { get; set; } = default!;

    [Required]
    [StringLength(100)]
    public string Street { get; set; } = default!;

    [Required]
    [StringLength(10)]
    public string HouseNumber { get; set; } = default!;

    [StringLength(10)]
    public string? ApartmentNumber { get; set; }
}