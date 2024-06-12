using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.DTOs.Person;
using Shared.Enums;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PersonController(IPersonService personService, IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddPersonAsync([FromForm] PersonCreateDto personDto)
    {
        var userId = User.GetUserId();
        await personService.AddPersonAsync(userId, personDto);

        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPersonByIdAsync(Guid id)
    {
        var userId = User.GetUserId();
        var userRole = await userService.GetUserRoleByIdAsync(userId);
        var person = await personService.GetPersonByIdAsync(userId, userRole, id);

        return Ok(person);
    }

    [HttpGet("ByUserId")]
    public async Task<IActionResult> GetPeopleByUserIdAsync([FromQuery] Guid? id)
    {
        var userId = User.GetUserId();
        var userRole = await userService.GetUserRoleByIdAsync(userId);
        var userIdToFetch = userRole == Role.Admin && id.HasValue ? id.Value : userId;
        var people = await personService.GetPeopleByUserIdAsync(userIdToFetch);

        return Ok(people);
    }

    [HttpGet("Picture/{personId:guid}")]
    public async Task<IActionResult> GetPersonPictureByIdAsync(Guid personId)
    {
        var userId = User.GetUserId();
        var picture = await personService.GetPictureByPersonId(userId, personId);

        return File(picture, "image/jpeg");
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePersonAsync([FromBody] PersonUpdateDto personUpdateDto)
    {
        var userId = User.GetUserId();
        var userRole = await userService.GetUserRoleByIdAsync(userId);
        await personService.UpdatePersonAsync(userId, userRole, personUpdateDto);

        return Ok();
    }

    [HttpPut("Picture")]
    public async Task<IActionResult> UpdatePersonPictureAsync([FromForm] PersonPictureUpdateDto personPictureUpdateDto)
    {
        var userId = User.GetUserId();
        await personService.UpdatePersonPictureAsync(userId, personPictureUpdateDto);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePersonAsync(Guid id)
    {
        var userId = User.GetUserId();
        var userRole = await userService.GetUserRoleByIdAsync(userId);
        await personService.DeletePersonAsync(userId, userRole, id);

        return Ok();
    }

    [HttpDelete("Picture/{personId:guid}")]
    public async Task<IActionResult> DeletePersonPictureAsync(Guid personId)
    {
        var userId = User.GetUserId();
        await personService.DeletePersonPictureAsync(userId, personId);

        return Ok();
    }

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("Admin/{id:guid}")]
    public async Task<IActionResult> GetPersonAsAdminByIdAsync(Guid id)
    {
        var person = await personService.GetPersonAsAdminByIdAsync(id);
        return Ok(person);
    }
}