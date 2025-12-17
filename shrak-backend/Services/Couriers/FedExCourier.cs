using Shrak.Models;

namespace Shrak.Services.Couriers;

public class FedExCourier(Settings settings, ILogger<FedExCourier> logger) : ICourier
{
    private readonly Settings _settings = settings;
    private readonly ILogger<FedExCourier> _logger = logger;

    public CourierType CourierType => CourierType.FedEx;
    
    public Task<string> GetBearerTokenAsync()
    {
        return Task.FromResult<string>("Poop"); 
    }

    public Task<TrackingInfo> GetTrackingInfoAsync(string? trackingNumber)
    {
        var trackingInfo = new TrackingInfo
        {
            Message = "FedEx Info",
            ErrorCode = "none",
            ErrorMessage = "none",
            RawJson = "big-blob-of-json",
        };
        return Task.FromResult(trackingInfo);
    }
}