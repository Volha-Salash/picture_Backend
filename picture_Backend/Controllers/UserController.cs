using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using picture_Backend.Data.Context;
using picture_Backend.Domain.Model;
using picture_Backend.Models;
using picture_Backend.Services;

namespace picture_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public UserController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var user = await _authenticationService.Login(request);

        if (user == null)
        {
            return BadRequest("Invalid username or password");
        }

        
        Response.Cookies.Append("userId", user.username.ToString());

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserDto request)
    {
        var user = await _authenticationService.Register(request);

        if (user == null)
        {
            return BadRequest("Failed to register user");
        }

        // Creating a new cookie with name "userId" and value as the user's ID
        Response.Cookies.Append("userId", user.Id.ToString());

        return Ok();
    }
}


/*  "Username": "jason",
 "Password": "password",
 "Email": "mysuperemail@gmail.com"
 *
 *{
 "Username": "new",
 "Password": "newpassword",
 "Email": "new@gmail.com"
}
 * 
 */
  