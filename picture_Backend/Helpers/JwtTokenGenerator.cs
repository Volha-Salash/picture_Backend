using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using picture_Backend.Domain.Model;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace picture_Backend.Helpers;

public class JwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _secretKey;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
    }

    public string GenerateToken(UserDto userDto)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, userDto.Username),
            new(ClaimTypes.Email, userDto.Email),
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
}
/*
private readonly string _secretKey = "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr";

public string GenerateToken(string userId, string username)
{
    var tokenGenerator = new JwtTokenGenerator();
    var token = tokenGenerator.GenerateToken(userId, username);
    
    // Проверка ключа
    var tokenHandler = new JwtSecurityTokenHandler();
    var secret = Encoding.ASCII.GetBytes(_secretKey);
    var validationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ValidateAudience = false,
        ValidateIssuer = false,
    };
    var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var rawValidatedToken);
    
    return token;
}

public bool VerifyToken(string token)
{
    // Проверка ключа
    var tokenHandler = new JwtSecurityTokenHandler();
    var secret = Encoding.ASCII.GetBytes(_secretKey);
    var validationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ValidateAudience = false,
        ValidateIssuer = false,
    };
    try
    {
        var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var rawValidatedToken);
        // Токен валиден
        return true;
    }
    catch (SecurityTokenException ex)
    {
        // Токен недействителен
        return false;
    }
}
}
*/
    
    
    
    /*
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _secretKey;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]));
    }

    public string GenerateToken(int userId, string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    /////////////////////////////
    /*
     * public class JwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _secretKey;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]));
    }

    public string GenerateToken(int userId, string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
     */
