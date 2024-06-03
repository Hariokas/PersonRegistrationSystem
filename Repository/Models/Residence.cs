namespace Repository.Models;

public class Residence
{
    public Guid Id { get; set; }
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string? ApartmentNumber { get; set; } = null;
    public Guid PersonId { get; set; }
    public Person Person { get; set; } = default!;
}