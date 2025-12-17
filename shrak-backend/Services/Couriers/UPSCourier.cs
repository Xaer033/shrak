using Shrak.Models;

namespace Shrak.Services.Couriers;

public class UPSCourier : ICourier
{
    public CourierType CourierType => CourierType.FedEx;
    
    public Task<string> GetBearerTokenAsync()
    {
        return Task.FromResult<string>("Poop"); 
    }

    public Task<TrackingInfo> GetTrackingInfoAsync(string? trackingNumber)
    {
        var trackingInfo = new TrackingInfo
        {
            Message = "UPS Info",
            ErrorCode = "none",
            ErrorMessage = "none",
            RawJson = "big-blob-of-json",
        };
        return Task.FromResult(trackingInfo);
    }
}