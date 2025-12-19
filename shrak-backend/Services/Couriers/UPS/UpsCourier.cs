using System.Net.Http.Headers;
using Newtonsoft.Json;
using Shrak.Models;
using Shrak.Protocols;

namespace Shrak.Services.Couriers;

public class UpsCourier : ICourier
{
    public CourierType CourierType => CourierType.UPS;
    
    private readonly ILogger<UpsCourier> _logger;
    private readonly HttpClient _http;
    private readonly IAuthService _auth;

    public UpsCourier(
        HttpClient http,
        IAuthService auth,
        ILogger<UpsCourier> logger)
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
        
        var upsResponse = JsonConvert.DeserializeObject<UpsTrackingResponse>(jsonResponse);

        var trackingInfo =  CreateTrackingInfoFromResponse(upsResponse);
        trackingInfo.ShipmentId = shipment.Id; 
        // trackingInfo.RawJson = jsonResponse;
        
        return trackingInfo;
    }

    private TrackingInfo CreateTrackingInfoFromResponse(UpsTrackingResponse response)
    {
        return new TrackingInfo();
    }
}