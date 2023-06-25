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
        var response = await _authenticationService.Login(request);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserDto request)
    {
        var response = await _authenticationService.Register(request);

        return Ok(response);
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
   /*
    [AllowAnonymous]
    [HttpPost("register/")]
    public async Task<ActionResult<UserDto>> RegisterUser(UserDto userDto)
    {  
        var config = new MapperConfiguration(cfg => cfg
            .CreateMap<UserDto, User>());
        var mapper = new Mapper(config); 
        var user = mapper.Map<User>(userDto);
        var createdUser = await _userRepository.RegisterAsync(user);

        if (createdUser == null)
        {
            return BadRequest();
        }
        var createdUserDto = mapper.Map<UserDto>(createdUser);
        var id = createdUser.Id;

        return CreatedAtAction(nameof(GetUserById), new { id }, createdUserDto);
    
    }
    */

 /*   [AllowAnonymous]
    [HttpPost("register/")]
    public async Task<ActionResult<UserDto>> RegisterUser(UserDto userDto)
    {  
        var config = new MapperConfiguration(cfg => cfg
            .CreateMap<UserDto, User>());
        var mapper = new Mapper(config); 
        var user = mapper.Map<User>(userDto);
        var createdUser = await _userRepository.RegisterAsync(user);
        if (createdUser == null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
    }
    
   
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
    
}
*/