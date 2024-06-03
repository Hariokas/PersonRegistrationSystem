using Shared.Enums;

namespace Repository.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.None;
    public ICollection<Person> People { get; set; } = [];
}