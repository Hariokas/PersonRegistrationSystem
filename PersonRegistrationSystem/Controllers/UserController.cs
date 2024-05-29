using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared;
using Shared.DTOs;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(UserAuthenticateDto user)
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

    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateUserAsync(UserAuthenticateDto user)
    {
        try
        {
            var existingUser = await userService.AuthenticateUserAsync(user);
            return Ok(existingUser);
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

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUserAsync(AdminUserDto user)
    {
        try
        {
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

    [HttpGet("{id}")]
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

    [HttpGet("{name}")]
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