using Shrak.Models;

namespace Shrak.Protocols;

public class GetAllShipmentsResponse : Response
{
    public IReadOnlyList<Shipment> ShipmentList { get; set; }
}