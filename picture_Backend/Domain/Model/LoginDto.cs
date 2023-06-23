using System.ComponentModel.DataAnnotations;

namespace picture_Backend.Domain.Model;

public class LoginDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}