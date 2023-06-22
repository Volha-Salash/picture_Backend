using System.Text.Json.Serialization;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

namespace picture_Backend.Models;
[Table("Users")]
public class User : IdentityUser
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    [JsonIgnore]
    public string Password { get; set; }

    public List<Image> Images { get; set; }
}