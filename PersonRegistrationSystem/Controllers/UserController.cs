using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.DTOs.User;
using Shared.Enums;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, ITokenService tokenService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserAuthenticateDto user)
    {
        var newUser = await userService.AddUserAsync(user);

        return Ok(newUser);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> AuthenticateUserAsync([FromBody] UserAuthenticateDto user)
    {
        var existingUser = await userService.AuthenticateUserAsync(user);
        var existingUserId = await userService.GetUserIdByNameAsync(existingUser.Username);
        var token = tokenService.GenerateToken(existingUserId, existingUser.Username);

        return Ok(new { Token = token });
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteUserAsync([FromBody] AdminUserDto user)
    {
        var userId = User.GetUserId();
        var userRole = await userService.GetUserRoleByIdAsync(userId);

        if (userRole != Role.Admin)
        {
            var userName = User.GetUserName();
            if (userName != user.Username) return BadRequest("You can only delete your own account");

            user.Id = userId;
        }

        var deletedUser = await userService.DeleteUserAsync(user);

        return Ok(deletedUser);
    }

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("ByUserID/{id:guid}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);

        return Ok(user);
    }

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("ByUserName/{name}")]
    public async Task<IActionResult> GetUserByNameAsync(string name)
    {
        var user = await userService.GetUserByNameAsync(name);

        return Ok(user);
    }
}