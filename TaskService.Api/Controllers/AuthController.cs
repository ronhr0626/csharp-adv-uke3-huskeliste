using Microsoft.AspNetCore.Mvc;
using TaskService.Api.Data;
using TaskService.Api.DTO;
using TaskService.Api.Models;
using TaskService.Api.Services;

namespace TaskService.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenReader _tokenReader;
    private readonly IPasswordHasher _passwordHasher;
    private readonly TaskDbContext _db;

    public AuthController(ITokenReader tokenReader, IPasswordHasher passwordHasher, TaskDbContext db)
    {
        _tokenReader = tokenReader;
        _passwordHasher = passwordHasher;
        _db = db;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password are required");
        }

        var username = request.Username.Trim();

        if (_db.Users.Any(u => u.Username == username))
        {
            return Conflict("Username is already taken");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = _passwordHasher.Hash(request.Password)
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        // Never return the password hash to the client
        return Created($"/auth/users/{user.Id}", new { user.Id, user.Username });
    }

    [HttpGet("token")]
    public IActionResult ReadToken()
    {
        var result = _tokenReader.ReadToken(Request.Headers.Authorization.ToString());

        if (result is null)
        {
            return BadRequest("Invalid or unreadable token");
        }

        return Ok(result);
    }
}
