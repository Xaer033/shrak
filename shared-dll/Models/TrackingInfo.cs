namespace Shrak.Models;

public class TrackingInfo
{
    public Shipment? Shipment { get; set; }
    public string? TransactionId { get; set; }
    public string? Message { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? RawJson { get; set; }
}