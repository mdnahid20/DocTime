using DocTime.Model;
using DocTime.Dtos;
using Microsoft.AspNetCore.Identity.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using System.Security.Claims;

namespace DocTime.Controllers;


public class AuthController : ApiController
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Username or Password is missing");
        }
        
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (existingUser != null)
            return BadRequest("Username already exists.");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Username or Password is missing");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var claims = new[]
       {
         new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
         new Claim(JwtRegisteredClaimNames.Jti, Guid. NewGuid().ToString()),
        new Claim("UserId", user.Id.ToString()),
        new Claim("Name", user.Username.ToString())
        // Add more claims if necessary
       };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); 
        var token = new JwtSecurityToken(
        _configuration["Jwt:Issuer"],
        _configuration["Jwt:Audience"],
        claims,
        expires: DateTime.UtcNow.AddMinutes(60),
        signingCredentials: signIn
        );
        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token); 
        return Ok(new { Token = tokenValue, User = user });
    }

    private string GenerateJwtToken(User user)
    {
         var claims = new[]
       {
         new Claim(ClaimTypes.Name, user.Username),
        // Add more claims if necessary
       };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiresInMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
