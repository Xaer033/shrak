using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shrak.Models;
using Shrak.Protocols;

namespace Shrak.Services.Couriers;

public class FedExAuthService : CachedAuthService, ICourierAuthService 
{
    private readonly HttpClient _http;
    private readonly CourierApiOptions _options;
    
    public CourierType CourierType => CourierType.FedEx;

    public FedExAuthService(
        HttpClient http,
        Dictionary<string, CourierApiOptions> options)
    {
        _http = http;
        _options = options["FedEx"];
    }

    protected override async Task<CourierOAuthTokenResponse> RequestCourierAccessTokenAsync(CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            _options.TokenUrl)
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
        var token = JsonConvert.DeserializeObject<CourierOAuthTokenResponse>(responseString);
        
        return token;
    }
}