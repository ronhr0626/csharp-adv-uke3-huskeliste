namespace TaskService.Api.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;

    // Only the BCrypt hash is stored, never the password itself
    public string PasswordHash { get; set; } = string.Empty;
}
