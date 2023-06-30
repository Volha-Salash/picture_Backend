using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using picture_Backend.Domain.Model;
using picture_Backend.Models;
/*
namespace picture_Backend.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthenticationService(IUserRepository userRepository,IConfiguration configuration )
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    //private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    public async Task<string> Register(UserDto request)
    {
        var userByEmail = await _userRepository.FindByEmailAsync(request.Email);
        var userByUsername = await _userRepository.FindByNameAsync(request.Username);
        if (userByEmail is not null || userByUsername is not null)
        {
            throw new ArgumentException($"User with email {request.Email} or username {request.Username} already exists.");
        }

        var result = await _userRepository.CreateAsync(request.Username, request.Password, request.Email);

        if (!result)
        {
            throw new ArgumentException($"Unable to register user {request.Username}");
        }

        return await Login(new LoginDto { Username = request.Username, Password = request.Password });
    }
    
   
    public async Task<string> Login(LoginDto request)
    {
        var user = await _userRepository.FindUserAsync(request.Username);

        if (user is null || !await _userRepository.CheckPasswordAsync(user, request.Password))
        {
            throw new ArgumentException($"Unable to authenticate user {request.Username}");
        }

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var identity = new ClaimsIdentity(authClaims, "UserAuthentication");

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return user.Username;
    }

}
*/