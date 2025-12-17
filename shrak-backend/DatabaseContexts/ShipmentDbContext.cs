using Shrak.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Shrak.DatabaseContexts;


public interface IShipmentDbContext
{
    public DbSet<Shipment> Shipments { get; }
    // -- Boiler plate --
    DatabaseFacade Database { get; }
    
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
public class ShipmentDbContext : DbContext, IShipmentDbContext
{
    public DbSet<Shipment> Shipments { get; set; }
    
    public ShipmentDbContext(DbContextOptions<ShipmentDbContext> options) : base(options) { }
}