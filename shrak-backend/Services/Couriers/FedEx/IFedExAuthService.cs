namespace Shrak.Services.Couriers;

public interface IFedExAuthService
{
    Task<string> GetAccessTokenAsync(CancellationToken ct = default);
}