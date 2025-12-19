using Shrak.Models;

namespace Shrak.Services.Couriers;

public class UPSCourier : ICourier
{
    public CourierType CourierType => CourierType.UPS;
    
    public Task<TrackingInfo> GetTrackingInfoAsync(Shipment shipment)
    {
        var trackingInfo = new TrackingInfo();
        return Task.FromResult(trackingInfo);
    }
}