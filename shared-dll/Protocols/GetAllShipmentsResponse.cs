using Shrak.Models;

namespace Shrak.Protocols;

public class GetAllShipmentsResponse : Response
{
    public List<Shipment>? ShipmentList { get; set; }
}