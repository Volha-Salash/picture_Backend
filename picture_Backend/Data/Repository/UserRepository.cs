using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using picture_Backend;
using picture_Backend.Data.Context;
using picture_Backend.Domain.Model;
using picture_Backend.Helpers;
using picture_Backend.Models;

public class UserRepository : IUserRepository
{
    private readonly ImageContext _dbConnection;
    private readonly IPasswordHasher<User> _passwordHasher;
    public UserRepository(ImageContext imageContext, IPasswordHasher<User> passwordHasher)
    {
        _dbConnection = imageContext;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<bool> CreateAsync(string username, string password, string email)
    {
        var connection = _dbConnection.CreateConnection();
        var hashedPassword = _passwordHasher.HashPassword(new User(), password);
        var rowsAffected = await connection.ExecuteAsync("INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email)",
            new { Username = username, Password = hashedPassword, Email = email });
        return rowsAffected == 1;
    }
    
    public async Task<User> FindByEmailAsync(string email)
    {
        var parameters = new { Email = email };
        const string sql = "SELECT * FROM [Users] WHERE [Email] = UPPER(@Email)";
        var connection = _dbConnection.CreateConnection();
        
        return await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);
    }
    

    public async Task<User> FindByNameAsync(string username)
    {
        var parameters = new { Username = username };
        const string sql = "SELECT * FROM [Users] WHERE [Username] = UPPER(@Username)";
        var connection = _dbConnection.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);
    }

    public async Task<User> FindUserAsync(string usernameOrEmail)
    {
        var parameters = new { UserNameOrEmail = usernameOrEmail };
        const string sql = "SELECT * FROM [Users] WHERE [Username] = UPPER(@UserNameOrEmail) OR [Email] = UPPER(@UserNameOrEmail)";
        var connection = _dbConnection.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        return result == PasswordVerificationResult.Success;
    }
    
}/*
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using picture_Backend;
using picture_Backend.Data.Context;
using picture_Backend.Domain.Model;
using picture_Backend.Helpers;
using picture_Backend.Models;

public class UserRepository : IUserRepository
{
    private readonly ImageContext _dbConnection;

    public UserRepository(ImageContext imageContext, IConfiguration configuration)
    {
        _dbConnection = imageContext;
        _configuration = configuration;
    }
    private readonly IConfiguration _configuration;
    

    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    /*
    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var connection = _dbConnection.CreateConnection();
        var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Username = @Username", 
            new { Username = username });
        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if (result != PasswordVerificationResult.Success)
        {
            var loginDto = new UserDto
            {
                Username = user.Username,
                Email = user.Email
            };

            var jwtTokenGenerator = new JwtTokenGenerator(_configuration);
            var token = jwtTokenGenerator.GenerateToken(loginDto);

            return token;
        }

        return null;
    }
    */
/*  
    public async Task<User> FindByUsername(string username)
    {
        var connection = _dbConnection.CreateConnection();

        var user = await connection.QueryFirstOrDefaultAsync<User>($"SELECT * FROM Users WHERE UserName = @Username", new { Username = username });

        return user;
    }
    public async Task<User> FindByNameAsync(string username)
    {
        var connection = _dbConnection.CreateConnection();

        var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE UserName = @Username", new { Username = username });

        return user;
    }
    public async Task<User> FindByEmailAsync(string email)
    {
        var connection = _dbConnection.CreateConnection();

        var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email", new { Email = email });

        return user;
    }


    

  /*  
    public async Task <string> AuthenticateAsync(string username, string password) { 
        var connection = _dbConnection.CreateConnection(); 
        var user = await connection.QueryFirstOrDefaultAsync("SELECT * FROM Users WHERE Username = @Username", 
            new { Username = username });
        if (user == null)
        {
            return null;
        }


        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if (result != PasswordVerificationResult.Success)
        {
            return null;
        }

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        var tokenGenerator = new JwtTokenGenerator(configuration);
        var token = tokenGenerator.GenerateToken(user.Id, user.Username);
        return token;
    }
    
/*
   
    */
    /*
    public async Task<bool> RegisterAsync(string username, string password, string email)
    {
        var connection = _dbConnection.CreateConnection();
        var hashedPassword = _passwordHasher.HashPassword(new User(), password);
        var rowsAffected = await connection.ExecuteAsync("INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email)", 
            new { Username = username, Password = hashedPassword, Email = email });
        return rowsAffected == 1;
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    { var connection = _dbConnection.CreateConnection();
        var users = await connection.GetAllAsync<User>();
        return users;
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
    
}
*/