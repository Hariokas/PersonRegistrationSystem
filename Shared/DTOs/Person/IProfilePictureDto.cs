using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Person;

public interface IProfilePictureDto
{
    IFormFile ProfilePicture { get; set; }
}