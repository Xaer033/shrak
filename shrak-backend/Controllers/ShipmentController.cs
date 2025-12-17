using Microsoft.AspNetCore.Mvc;
using Shrak.Controllers;
using Shrak.DatabaseContexts;
using Shrak.Protocols;
using Shrak.Services;

namespace LaneCheckServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ShipmentController : StateController   
{
    private readonly IShipmentService _laneService;
    
    public ShipmentController(
        IShipmentService laneService,
        IShipmentDbContext dbContext,
        ILogger<StateController> logger) : base(dbContext, logger)
    {
        _laneService = laneService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllShipments([FromBody] GetAllShipmentsRequest request)
    {
        var response = new GetAllShipmentsResponse();
        
        var actionResult = await ProcessRequestAsync(request, response, async () =>
        {
            response.ShipmentList = await _laneService.GetAllShipments();
        });
        
        return actionResult;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddShipment([FromBody] AddShipmentRequest request)
    {
        var response = new AddShipmentResponse();
        
        var actionResult = await ProcessRequestAsync(request, response, async () =>
        {
            response.Shipment = await _laneService.AddShipment(request.TrackingNumber);
        });
        
        return actionResult;
    }
    
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveShipment([FromBody] RemoveShipmentRequest request)
    {
        var response = new RemoveShipmentResponse();
        
        var actionResult = await ProcessRequestAsync(request, response, async () =>
        {
            await _laneService.RemoveShipment(request.ShipmentId);
        });
        
        return actionResult;
    }
}