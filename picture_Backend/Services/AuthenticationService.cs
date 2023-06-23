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
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    public async Task<string> Login(LoginDto request)
    {
        var user = await FindByUsernameOrEmail(request.Username);

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
        if (result != PasswordVerificationResult.Success)
        {
            return null;
        }

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = GetToken(authClaims);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return token;
    }

    private async Task<User> FindByUsernameOrEmail(string usernameOrEmail) 
    {
        var user = await _userRepository.FindByNameAsync(usernameOrEmail);

        if (user is null)
        {
            user = await _userRepository.FindByEmailAsync(usernameOrEmail);
        }

        return user;
    }
}