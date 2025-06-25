using Microsoft.EntityFrameworkCore;
using WareTrack.Web.Models;

namespace WareTrack.Web.Data;

public class DataContext : DbContext
{
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Shelf> Shelves { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options) {}
}