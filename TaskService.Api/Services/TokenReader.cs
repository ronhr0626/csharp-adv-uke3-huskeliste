using System.IdentityModel.Tokens.Jwt;
using TaskService.Api.Models;

namespace TaskService.Api.Services;

public class TokenReader : ITokenReader
{
    private const string BearerPrefix = "Bearer ";

    public TokenInfo? ReadToken(string authHeader)
    {
        // The header must exist and start with "Bearer "
        if (string.IsNullOrWhiteSpace(authHeader) ||
            !authHeader.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        // Remove "Bearer " (any casing) and keep only the token itself
        var token = authHeader[BearerPrefix.Length..].Trim();

        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
        {
            return null;
        }

        var jwt = handler.ReadJwtToken(token);

        return new TokenInfo
        {
            Header = jwt.Header.ToDictionary(item => item.Key, item => item.Value),
            Claims = jwt.Claims
                .Select(claim => new TokenInfo.TokenClaim { Type = claim.Type, Value = claim.Value })
                .ToList()
        };
    }

    public Guid? GetUserId(string authHeader)
    {
        var tokenInfo = ReadToken(authHeader);
        if (tokenInfo is null)
        {
            return null;
        }

        // "sub" (subject) is the claim that holds the user id
        var userIdClaim = tokenInfo.Claims.FirstOrDefault(claim => claim.Type == "sub");

        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return null;
        }

        return userId;
    }
}
