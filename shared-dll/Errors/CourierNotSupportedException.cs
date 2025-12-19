using Shrak.Models;

namespace Shrak.Errors;

public class UnsupportedCourierException : Exception
{
    public CourierType CourierType { get; }

    public UnsupportedCourierException(CourierType courierType)
        : base($"Courier type '{courierType}' is not supported.")
    {
        CourierType = courierType;
    }
}
