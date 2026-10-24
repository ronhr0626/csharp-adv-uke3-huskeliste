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
}