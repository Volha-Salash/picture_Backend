using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using picture_Backend.Domain.Model;
using picture_Backend.Models;

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

    if(user is null || !await _userRepository.CheckPasswordAsync(user, request.Password))
    {
        throw new ArgumentException($"Unable to authenticate user {request.Username}");
    }

    var authClaims = new List<Claim>
    {   new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        
    };

    var token = GetToken(authClaims);
    
    VerifyToken(token);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
{
    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

    var token = new JwtSecurityToken(
        issuer: _configuration["JWT:ValidIssuer"],
        audience: _configuration["JWT:ValidAudience"],
        expires: DateTime.Now.AddMinutes(15),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

    return token;
    
}
private async Task VerifyToken(JwtSecurityToken token)
{
    try
    {
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero
        };
        
        SecurityToken validatedToken;
        ClaimsPrincipal claimsPrincipal = handler.ValidateToken(token.RawData, validationParameters, out validatedToken);
        Console.WriteLine("Token is valid");
    }
    catch(Exception ex)
    {
        Console.WriteLine("Token validation failed: " + ex.Message);
       
    }
}
/*
private async Task<ClaimsPrincipal> VerifyToken(string tokenString)
{
   
    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = _configuration["JWT:ValidIssuer"],
        ValidateAudience = true,
        ValidAudience = _configuration["JWT:ValidAudience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
    };

    try
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claimsPrincipal = tokenHandler.ValidateToken(tokenString, tokenValidationParameters, out var validToken);

        Console.WriteLine("Token is valid");
        return claimsPrincipal;
    }
    catch(Exception ex)
    {
        Console.WriteLine("Token validation failed: " + ex.Message);
        return null;
    }
}
*/

}