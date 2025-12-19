using Newtonsoft.Json;

namespace Shrak.Protocols;

public class OAuthTokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = null!;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonProperty("scope")]
    public string Scope { get; set; }
    
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
}