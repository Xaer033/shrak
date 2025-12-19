using Newtonsoft.Json;
using Shrak.Models;
using Shrak.Protocols;

namespace Shrak.Services.Couriers;

public abstract class CachedAuthService 
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    protected CachedJwt _cachedJwt;
    
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

            var token = await RequestCourierAccessTokenAsync(ct);

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

    protected virtual Task<CourierOAuthTokenResponse> RequestCourierAccessTokenAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}