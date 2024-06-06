using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models;

public class Residence
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    [StringLength(60)]
    public string City { get; set; } = string.Empty;

    [StringLength(100)]
    public string Street { get; set; } = string.Empty;

    [StringLength(10)]
    public string HouseNumber { get; set; } = string.Empty;

    [StringLength(10)]
    public string? ApartmentNumber { get; set; } = null;

    public Guid PersonId { get; set; }
    public Person Person { get; set; } = default!;
}