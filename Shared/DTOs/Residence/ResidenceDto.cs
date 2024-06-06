namespace Shared.DTOs.Residence;

public class ResidenceDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }
    public string? ApartmentNumber { get; set; }
}