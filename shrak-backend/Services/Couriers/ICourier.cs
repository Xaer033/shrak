using Shrak.Models;

namespace Shrak.Services.Couriers;

public interface ICourier
{
    CourierType CourierType { get; }
    Task<string> GetBearerTokenAsync();
    Task<TrackingInfo> GetTrackingInfoAsync(string? trackingNumber);
}