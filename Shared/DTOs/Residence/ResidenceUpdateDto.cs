using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Residence;

public class ResidenceUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public string? HouseNumber { get; set; }

    public string? ApartmentNumber { get; set; }
}