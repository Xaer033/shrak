using Shrak.Errors;
using Shrak.Models;

namespace Shrak.Services.Couriers;

public class CourierFactory
{
    private readonly IEnumerable<ICourier> _couriers;
    
    public CourierFactory(IEnumerable<ICourier> couriers)
    {
        _couriers = couriers;
    }
    public ICourier GetCourier(CourierType courierType)
    {
        return _couriers.FirstOrDefault(e => e.CourierType == courierType)
               ?? throw new UnsupportedCourierException(courierType); 
    }
}