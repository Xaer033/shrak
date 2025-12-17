using Microsoft.AspNetCore.Mvc;
using Shrak.Controllers;
using Shrak.DatabaseContexts;
using Shrak.Protocols;
using Shrak.Services;

namespace LaneCheckServer.Controllers;

[ApiController]
[Route("shipments")]
public class ShipmentController : StateController   
{
    private readonly IShipmentService _shipmentService;
    
    public ShipmentController(
        IShipmentService shipmentService,
        IShipmentDbContext dbContext,
        ILogger<StateController> logger) : base(dbContext, logger)
    {
        _shipmentService = shipmentService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllShipments([FromBody] GetAllShipmentsRequest request)
    {
        var response = new GetAllShipmentsResponse();
        
        var actionResult = await ProcessRequestAsync(request, response, async () =>
        {
            response.ShipmentList = await _shipmentService.GetAllShipments();
        });
        
        return actionResult;
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshShipmentTracking([FromBody] RefreshShipmentTrackingRequest request)
    {
        var response = new RefreshShipmentTrackingResponse();
        
        var actionResult = await ProcessRequestAsync(request, response, async () =>
        {
            response.TrackingInfoList = await _shipmentService.RefreshAllShipments();
        });
        
        return actionResult;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddShipment([FromBody] AddShipmentRequest request)
    {
        var response = new AddShipmentResponse();
        
        var actionResult = await ProcessRequestAsync(request, response, async () =>
        {
            response.Shipment = await _shipmentService.AddShipment(request.TrackingNumber, request.OverrideCourierType);
        });
        
        return actionResult;
    }
    
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveShipment([FromBody] RemoveShipmentRequest request)
    {
        var response = new RemoveShipmentResponse();
        
        var actionResult = await ProcessRequestAsync(request, response, async () =>
        {
            await _shipmentService.RemoveShipment(request.ShipmentId);
        });
        
        return actionResult;
    }
}