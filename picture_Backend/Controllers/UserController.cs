using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using picture_Backend.Data.Context;
using picture_Backend.Domain.Model;
using picture_Backend.Models;
using picture_Backend.Services;

namespace picture_Backend.Controllers;

//[Authorize]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;

    public UserController(IUserRepository userRepository, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        //var token = await _userRepository.AuthenticateAsync(model.Username, model.Password);
        var token = await _authenticationService.Login(loginDto);
        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterUser([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var isRegistered = await _userRepository.RegisterAsync(userDto.Username, userDto.Password, userDto.Email);
        if (isRegistered)
        {
            return Ok();
        }

        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return Ok(users);

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