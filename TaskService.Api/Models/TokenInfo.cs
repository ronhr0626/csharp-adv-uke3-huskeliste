namespace TaskService.Api.Models;

public class TokenInfo
{
    public Dictionary<string, object> Header { get; set; } = [];
    public List<TokenClaim> Claims { get; set; } = [];

    public class TokenClaim
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}