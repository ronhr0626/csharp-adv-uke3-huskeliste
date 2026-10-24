using TaskService.Api.Models;

namespace TaskService.Api.Services;

public interface ITokenReader
{
    TokenInfo? ReadToken(string authHeader);
    Guid? GetUserId(string authHeader);
}
