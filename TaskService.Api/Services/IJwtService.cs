using TaskService.Api.Models;

namespace TaskService.Api.Services;

public interface IJwtService
{
    string CreateToken(User user);
}
