using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Person;

public class PersonPictureUpdateDto
{
    public Guid Id { get; set; }
    public IFormFile ProfilePicture { get; set; } = default!;
}