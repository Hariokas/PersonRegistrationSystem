using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.DTOs.Residence;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ResidenceController(IResidenceService residenceService, IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddResidenceAsync([FromForm] ResidenceCreateDto residenceDto)
    {
        var userId = User.GetUserId();
        await residenceService.AddResidenceAsync(userId, residenceDto);

        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetResidenceByIdAsync(Guid id)
    {
        var userId = User.GetUserId();
        var userRole = await userService.GetUserRoleByIdAsync(userId);
        var residence = await residenceService.GetResidenceByIdAsync(userId, userRole, id);

        return Ok(residence);
    }

    [HttpGet("ByPersonID")]
    public async Task<IActionResult> GetResidenceByPersonIdAsync([FromQuery] Guid id)
    {
        var userId = User.GetUserId();
        var userRole = await userService.GetUserRoleByIdAsync(userId);
        var residences = await residenceService.GetResidenceByPersonIdAsync(userId, userRole, id);

        return Ok(residences);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateResidenceAsync([FromForm] ResidenceUpdateDto residenceDto)
    {
        var userId = User.GetUserId();
        var userRole = await userService.GetUserRoleByIdAsync(userId);
        await residenceService.UpdateResidenceAsync(userId, userRole, residenceDto);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteResidenceAsync(Guid id)
    {
        var userId = User.GetUserId();
        var userRole = await userService.GetUserRoleByIdAsync(userId);
        await residenceService.DeleteResidenceAsync(userId, userRole, id);

        return Ok();
    }
}