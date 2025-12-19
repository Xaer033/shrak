using System.ComponentModel.DataAnnotations;

namespace Shrak.Models;

public class Shipment
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? TrackingNumber { get; set; }
    public string? Nickname { get; set; }
    public CourierType CourierType { get; set; }
    public ShipmentStatus Status { get; set; }
    public DateTime CreatedTimestamp { get; set; }
    public DateTime LastUpdated { get; set; }


    public override string ToString()
    {
        return $"Shipment {Id} | Name: {Nickname} | Tracking #: {TrackingNumber}";
    }
}

public enum ShipmentStatus 
{
    None,
    Unknown,
    ShipmentReceived,
    OnTheWay,
    OutForDelivery,
    Delivered,
}

public enum CourierType
{
    None,
    UPS,
    FedEx,
    USPS,
    DHL,
    Amazon,
}