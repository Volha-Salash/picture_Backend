using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using picture_Backend.Domain.Model;
using picture_Backend.Helpers;
using picture_Backend.Models;


namespace picture_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IDbConnection _dbConnection;

    public AuthController(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _dbConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Username = @Username",
            new { Username = username });
        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result != PasswordVerificationResult.Success)
        {
            return null;
        }

        var tokenGenerator = new JwtTokenGenerator("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr");
        var token = tokenGenerator.GenerateToken(user.Id, user.Username);
        return token;
    }
}
