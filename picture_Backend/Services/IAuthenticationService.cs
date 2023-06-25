using picture_Backend.Domain.Model;

namespace picture_Backend.Services;

public interface IAuthenticationService
{
    Task<string> Login(LoginDto request);
    Task<string> Register(UserDto request);
}