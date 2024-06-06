using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User;

public class UserAuthenticateDto
{
    [Required]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 20 characters")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    public string Password { get; set; } = string.Empty;
}