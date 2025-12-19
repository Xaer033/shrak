using Newtonsoft.Json;
using Shrak.Models;
using Shrak.Protocols;

namespace Shrak.Services.Couriers;

public class UpsAuthService : CachedAuthService, ICourierAuthService 
{
    private readonly HttpClient _http;
    private readonly CourierApiOptions _options;

    public CourierType CourierType => CourierType.UPS;
    
    public UpsAuthService(
        HttpClient http,
        Dictionary<string, CourierApiOptions> options)
    {
        _http = http;
        _options = options["UPS"];
    }

    protected override Task<CourierOAuthTokenResponse> RequestCourierAccessTokenAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
