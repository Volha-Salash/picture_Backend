
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using picture_Backend;
using picture_Backend.Data.Context;
using picture_Backend.Domain.Model;
using picture_Backend.Helpers;
using picture_Backend.Models;

public class UserRepository : IUserRepository
{
    private readonly ImageContext _dbConnection;

    public UserRepository(ImageContext imageContext)
    {
        _dbConnection = imageContext;
    }
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var connection = _dbConnection.CreateConnection();
        var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Username = @Username", new { Username = username });
        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if (result != PasswordVerificationResult.Success)
        {
            return null;
        }

        var tokenGenerator = new JwtTokenGenerator("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr");
        var token = tokenGenerator.GenerateToken(user.Id, user.Username);
        return token;
    }
    
    public async Task<bool> RegisterAsync(string username, string password, string email)
    {
        var connection = _dbConnection.CreateConnection();
        var hashedPassword = _passwordHasher.HashPassword(new User(), password);
        var rowsAffected = await connection.ExecuteAsync("INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email)", 
            new { Username = username, Password = hashedPassword, Email = email });
        return rowsAffected == 1;
    }
    
/*
    public async Task<User> GetByIdAsync(int id)
    {   var connection = _dbConnection.CreateConnection();
        return await connection.GetAsync<User>(id);
    }
    
    public async Task<User> GetByUsernameAsync(string username)
    { var connection = _dbConnection.CreateConnection();
        var query = "SELECT * FROM Users WHERE Username = @Username";
        return await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
    }
 
    

/*
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbConnection.GetAllAsync<User>();
    }
    */

/*  public async Task<User> GetByUsernameAsync(string username)
   { 
       var connection = _dbConnection.CreateConnection();
       var query = "SELECT * FROM Users WHERE Username = @UserUsername";
       return await connection.QueryFirstOrDefaultAsync<User>(query, new { UserUsername = username });
   }
   */

/*
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        return await _dbConnection.DeleteAsync(user);
    }
    */
}