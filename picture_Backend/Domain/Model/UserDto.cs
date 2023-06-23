using System.ComponentModel.DataAnnotations;

namespace picture_Backend.Domain.Model;

public class UserDto
{   
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}
