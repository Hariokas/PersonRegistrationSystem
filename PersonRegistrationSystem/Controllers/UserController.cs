using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared;
using Shared.DTOs;
using Shared.Enums;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserService userService, ITokenService tokenService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserAuthenticateDto user)
    {
        try
        {
            var newUser = await userService.AddUserAsync(user);
            return Ok(newUser);
        }
        catch (UserValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidCredentialsException e)
        {
            return BadRequest(e.Message);
        }
        catch (UserAlreadyExistsException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> AuthenticateUserAsync([FromBody] UserAuthenticateDto user)
    {
        try
        {
            var existingUser = await userService.AuthenticateUserAsync(user);
            var existingUserId = await userService.GetUserIdByNameAsync(existingUser.Username);

            var token = tokenService.GenerateToken(existingUserId, existingUser.Username);
            return Ok(new { Token = token });
        }
        catch (InvalidCredentialsException e)
        {
            return BadRequest(e.Message);
        }
        catch (UserNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteUserAsync([FromBody] AdminUserDto user)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userIdClaim!.Value);

            var userRole = await userService.GetUserRoleByIdAsync(userId);
            if (userRole != Role.Admin)
            {
                var userNameClaim = User.FindFirst(ClaimTypes.Name);
                var userName = userNameClaim!.Value;

                if (userName != user.Username)
                    return BadRequest("You can only delete your own account");

                user.Id = userId;
            }

            var deletedUser = await userService.DeleteUserAsync(user);
            return Ok(deletedUser);
        }
        catch (InvalidCredentialsException e)
        {
            return BadRequest(e.Message);
        }
        catch (UserNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("By-ID/{id}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (UserNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("By-Name/{name}")]
    public async Task<IActionResult> GetUserByNameAsync(string name)
    {
        try
        {
            var user = await userService.GetUserByNameAsync(name);
            return Ok(user);
        }
        catch (UserNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}