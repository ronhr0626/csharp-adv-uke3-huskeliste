namespace TaskService.Api.Services;

public class PasswordHasher : IPasswordHasher
{
    // Turns a plain password into a BCrypt hash (with a random salt built in)
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Checks a plain password against a stored hash
    public bool Verify(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
