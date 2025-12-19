using Shrak.Models;

namespace Shrak.Protocols;

public class RefreshShipmentTrackingResponse : Response
{
    public IReadOnlyList<TrackingInfo>? TrackingInfoList { get; set; }
}