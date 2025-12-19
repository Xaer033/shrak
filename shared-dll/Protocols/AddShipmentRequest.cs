using Shrak.Models;

namespace Shrak.Protocols;

public class AddShipmentRequest : Request
{
    public required string TrackingNumber { get; set; }
    public string Nickname { get; set; }
    public CourierType OverrideCourierType { get; set; }
}