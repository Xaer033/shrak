using Shrak.Models;

namespace Shrak.Services.Couriers;

public interface ICourier
{
    CourierType CourierType { get; }
    Task<TrackingInfo> GetTrackingInfoAsync(Shipment shipment);
}