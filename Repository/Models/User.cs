using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.None;
    public ICollection<Person> People { get; set; } = [];
}