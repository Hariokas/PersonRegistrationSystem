using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Person;

public class PersonPictureUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public IFormFile ProfilePicture { get; set; } = default!;
}