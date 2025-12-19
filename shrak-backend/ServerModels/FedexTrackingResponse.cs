using Newtonsoft.Json;

namespace Shrak.Protocols;

public class FedexTrackingResponse
{
    [JsonProperty("output")]
    public FedexOutput Output { get; set; }
}

public class FedexOutput
{
    [JsonProperty("completeTrackResults")]
    public List<FedexCompleteTrackResult> CompleteTrackResults { get; set; }
}

public class FedexCompleteTrackResult
{
    [JsonProperty("trackResults")]
    public List<FedexTrackResult> TrackResults { get; set; }
}

public class FedexTrackResult
{
    [JsonProperty("trackingNumberInfo")]
    public FedexTrackingNumberInfo TrackingNumberInfo { get; set; }

    [JsonProperty("latestStatusDetail")]
    public FedexLatestStatusDetail LatestStatusDetail { get; set; }

    [JsonProperty("dateAndTimes")]
    public List<FedexDateAndTime> DateAndTimes { get; set; }

    [JsonProperty("deliveryDetails")]
    public FedexDeliveryDetails DeliveryDetails { get; set; }
}

public class FedexTrackingNumberInfo
{
    [JsonProperty("trackingNumber")]
    public string TrackingNumber { get; set; }
}

public class FedexLatestStatusDetail
{
    [JsonProperty("statusByLocale")]
    public string Status { get; set; }

    [JsonProperty("ancillaryDetails")]
    public List<FedexAncillaryDetail> AncillaryDetails { get; set; }
}

public class FedexAncillaryDetail
{
    [JsonProperty("reasonDescription")]
    public string ReasonDescription { get; set; }
}

public class FedexDateAndTime
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("dateTime")]
    public DateTime DateTime { get; set; }
}

public class FedexDeliveryDetails
{
    [JsonProperty("actualDeliveryAddress")]
    public FedexAddress ActualDeliveryAddress { get; set; }
}

public class FedexAddress
{
    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("stateOrProvinceCode")]
    public string State { get; set; }
}
