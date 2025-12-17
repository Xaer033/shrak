using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Shrak.DatabaseContexts;
using Shrak.Errors;
using Shrak.Models;
using Shrak.Services.Couriers;

namespace Shrak.Services;

public interface IShipmentService
{
    Task<Shipment> AddShipment(string trackingNumber, CourierType overrideCourierType);
    Task RemoveShipment(int shipmentId);
    Task<IReadOnlyList<Shipment>> GetAllShipments();
    Task<IReadOnlyList<TrackingInfo>> RefreshAllShipments();
}

public class ShipmentService(IShipmentDbContext context, CourierFactory courierFactory) : IShipmentService
{
    public async Task<Shipment> AddShipment(string trackingNumber, CourierType overrideCourierType)
    {
        var now = DateTime.UtcNow;
        
        var courierType = overrideCourierType == CourierType.None 
            ? DetectCourierType(trackingNumber)
            : overrideCourierType;
        
        var shipment = new Shipment
        {
            TrackingNumber = trackingNumber,
            Status = ShipmentStatus.Unknown,
            CourierType = courierType, 
            CreatedTimestamp = now, 
            LastUpdated =  now,
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
    
    public async Task<IReadOnlyList<Shipment>> GetAllShipments()
    {
        return await context.Shipments.ToListAsync();
    }
    
    public async Task<IReadOnlyList<TrackingInfo>> RefreshAllShipments()
    { 
        var shipments = await GetSavedShipments();
        var tasks = shipments.Select(TrackSafelyAsync).ToList();
        return await Task.WhenAll(tasks);
    }

    private async Task<IEnumerable<Shipment>> GetSavedShipments()
    {
        return await context.Shipments.ToListAsync();
    }
    
    private async Task<TrackingInfo> TrackSafelyAsync(Shipment shipment)
    {
        var info = new TrackingInfo();
        try
        {
            var courier = courierFactory.GetCourier(shipment.CourierType);
            return await courier.GetTrackingInfoAsync(shipment.TrackingNumber);
        }
        catch (Exception ex)
        {
            info.ErrorMessage = ex.Message;
            return info;
        }
    }
    
    private static CourierType DetectCourierType(string trackingNumber)
    {
        var sanitizedTracking = trackingNumber.Replace(" ", "").ToUpperInvariant();

        // UPS
        if (Regex.IsMatch(sanitizedTracking, "^1Z[0-9A-Z]{16}$"))
            return CourierType.UPS;

        // DHL
        if (Regex.IsMatch(sanitizedTracking, "^JD\\d{18,20}$") ||
            Regex.IsMatch(sanitizedTracking, "^\\d{10}$"))
            return CourierType.DHL;

        // USPS
        if (Regex.IsMatch(sanitizedTracking, "^(94|93|92|95)\\d{18,20}$") ||
            Regex.IsMatch(sanitizedTracking, "^420\\d{20,22}$"))
            return CourierType.USPS;

        // FedEx
        if (Regex.IsMatch(sanitizedTracking, "^\\d{12}$|^\\d{15}$|^\\d{20,22}$|^\\d{34}$"))
            return CourierType.FedEx;

        return CourierType.None;
    }
}