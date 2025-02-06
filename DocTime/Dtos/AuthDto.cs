using System.ComponentModel.DataAnnotations;

namespace DocTime.Dtos;

public class AuthDto
{
    public string Username { get; set; }

    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }
}
