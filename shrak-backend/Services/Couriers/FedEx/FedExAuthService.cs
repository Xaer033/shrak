using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shrak.Models;
using Shrak.Protocols;

namespace Shrak.Services.Couriers;

public class FedExAuthService : IFedExAuthService
{
    private readonly HttpClient _http;
    private readonly CourierApiOptions _options;
    private CachedJwt? _cachedJwt;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public FedExAuthService(
        HttpClient http,
        Dictionary<string, CourierApiOptions> options)
    {
        _http = http;
        _options = options["FedEx"];
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken ct = default)
    {
        if (_cachedJwt is not null && !_cachedJwt.IsExpired)
            return _cachedJwt.Token;

        await _lock.WaitAsync(ct);
        try
        {
            // Double-check after acquiring lock
            if (_cachedJwt is not null && !_cachedJwt.IsExpired)
                return _cachedJwt.Token;

            var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = _options.ApiKey,
                    ["client_secret"] = _options.Secret
                })
            };

            var response = await _http.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync(ct);
            var token = JsonConvert.DeserializeObject<OAuthTokenResponse>(responseString);

            _cachedJwt = new CachedJwt()
            {
                Token = token!.AccessToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn)
            };

            return _cachedJwt.Token;
        }
        finally
        {
            _lock.Release();
        }
    }
}