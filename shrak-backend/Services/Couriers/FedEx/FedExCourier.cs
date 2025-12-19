using System.Net.Http.Headers;
using Newtonsoft.Json;
using Shrak.Models;
using Shrak.Protocols;

namespace Shrak.Services.Couriers;

public class FedExCourier : ICourier
{
    public CourierType CourierType => CourierType.FedEx;
    
    private readonly ILogger<FedExCourier> _logger;
    private readonly HttpClient _http;
    private readonly IAuthService _auth;

    public FedExCourier(
        HttpClient http,
        IAuthService auth,
        ILogger<FedExCourier> logger)
    {
        _http = http;
        _auth = auth;
        _logger = logger;
    }

    public async Task<TrackingInfo> GetTrackingInfoAsync(Shipment shipment)
    {
        var token = await _auth.GetOrRequestAccessTokenAsync(shipment.CourierType);
        
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/track/v1/trackingnumbers");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        request.Content = JsonContent.Create(new
        {
            trackingInfo = new[]
            {
                new { trackingNumberInfo = new { shipment.TrackingNumber } }
            }
        });

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
            
        _logger.LogInformation(jsonResponse);
        
        var fedexResponse = JsonConvert.DeserializeObject<FedexTrackingResponse>(jsonResponse);

        var trackingInfo =  CreateTrackingInfoFromResponse(fedexResponse);
        trackingInfo.ShipmentId = shipment.Id; 
        // trackingInfo.RawJson = jsonResponse;
        
        return trackingInfo;
    }

    private TrackingInfo CreateTrackingInfoFromResponse(FedexTrackingResponse fedexResponse)
    {
        var trackResult = fedexResponse
            ?.Output
            ?.CompleteTrackResults?
            .FirstOrDefault()
            ?.TrackResults?
            .FirstOrDefault();

        var deliveredDate = trackResult?
            .DateAndTimes?
            .FirstOrDefault(d => d.Type == "ACTUAL_DELIVERY")
            ?.DateTime;

        var model = new TrackingInfo
        {
            TrackingNumber = trackResult?.TrackingNumberInfo?.TrackingNumber,
            Status = trackResult?.LatestStatusDetail?.Status,
            DeliveredAt = deliveredDate,
            DeliveryCity = trackResult?.DeliveryDetails?.ActualDeliveryAddress?.City,
            DeliveryState = trackResult?.DeliveryDetails?.ActualDeliveryAddress?.State,
            LatestUpdateMessage = trackResult?
                .LatestStatusDetail?
                .AncillaryDetails?
                .FirstOrDefault()?
                .ReasonDescription
        };
        
        return model;
    }
}