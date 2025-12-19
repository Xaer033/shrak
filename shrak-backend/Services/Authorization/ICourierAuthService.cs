using Shrak.Models;

namespace Shrak.Services.Couriers;

public interface ICourierAuthService
{
    CourierType CourierType { get; }
    Task<string> GetAccessTokenAsync(CancellationToken ct = default);
}