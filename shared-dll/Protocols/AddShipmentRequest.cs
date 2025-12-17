namespace Shrak.Protocols;

public class AddShipmentRequest : Request
{
    public required string TrackingNumber { get; set; }
}