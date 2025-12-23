using Shrak.Models;

namespace Shrak.Protocols;

public class AddShipmentResponse : Response
{
    public Shipment Shipment { get; set; }
}