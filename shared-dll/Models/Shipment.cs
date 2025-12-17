using System.ComponentModel.DataAnnotations;

namespace Shrak.Models;

public class Shipment
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }
    public string? Nickname { get; set; }
    public ShipmentStatus Status { get; set; }
    public DateTime LastUpdated { get; set; }
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

public enum Carrier
{
    None,
    UPS,
    FedEx,
    USPS,
    DHL,
    Amazon,
}