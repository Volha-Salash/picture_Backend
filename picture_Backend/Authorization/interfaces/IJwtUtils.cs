using picture_Backend.Models;

namespace picture_Backend.Authorization.interfaces;

public interface IJwtUtils
{
    public string GenerateToken(User user);
    public int? ValidateToken(string token);
}