namespace Shrak.Models;

public class TrackingInfo
{
    public int ShipmentId { get; set; }
    public string TrackingNumber { get; set; }
    public string Status { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string DeliveryCity { get; set; }
    public string DeliveryState { get; set; }
    public string LatestUpdateMessage { get; set; }
    public string ExceptionMessage { get; set; }
    public string RawJson { get; set; }
}
