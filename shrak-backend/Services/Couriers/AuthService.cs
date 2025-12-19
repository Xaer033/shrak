using Shrak.Models;

namespace Shrak.Services.Couriers;

public interface IAuthService
{
    Task<string> GetAccessTokenAsync(CourierType courierType);
}

public class AuthService : IAuthService
{
    private readonly IDictionary<CourierType, ICourierAuthService> _authServices;

    public AuthService(IEnumerable<ICourierAuthService> authServices)
    {
        _authServices = authServices.ToDictionary(a => a.CourierType);
    }

    public Task<string> GetAccessTokenAsync(CourierType courierType)
    {
        if (!_authServices.TryGetValue(courierType, out var service))
            throw new NotSupportedException($"Carrier {courierType} not supported");

        return service.GetAccessTokenAsync();
    }
}