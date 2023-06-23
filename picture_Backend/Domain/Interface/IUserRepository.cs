using picture_Backend.Models;

namespace picture_Backend;

public interface IUserRepository
{
    //Task<IEnumerable<User>> GetAllAsync();
    //Task<User> GetByIdAsync(int id);
    //Task<User> GetByUsernameAsync(string username);
    Task<bool> RegisterAsync(string username, string password, string email);
    Task<string> AuthenticateAsync(string username, string password);

}