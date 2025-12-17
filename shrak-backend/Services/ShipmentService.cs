using Microsoft.EntityFrameworkCore;
using Shrak.DatabaseContexts;
using Shrak.Errors;
using Shrak.Models;

namespace Shrak.Services;

public interface IShipmentService
{
    public Task<Shipment> AddShipment(string shipmentName);
    public Task RemoveShipment(int shipmentId);
    public Task<List<Shipment>> GetAllShipments();
}

public class ShipmentService(IShipmentDbContext context) : IShipmentService
{
    public async Task<Shipment> AddShipment(string trackingNumber)
    {
        var shipment = new Shipment
        {
            TrackingNumber = trackingNumber,
            Status = ShipmentStatus.Unknown,
        };
        
        context.Shipments.Add(shipment);
        await context.SaveChangesAsync();
        
        return shipment;
    }

    public async Task RemoveShipment(int shipmentId)
    {
        var shipment = await context.Shipments.FindAsync(shipmentId);
        if (shipment == null)
            throw BackendException.Create<ShipmentNotFoundError>();
        
        context.Shipments.Remove(shipment);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<Shipment>> GetAllShipments()
    {
        return await context.Shipments.ToListAsync();
    }
}