using Microsoft.EntityFrameworkCore;
using WebMvc.Models;

public class BusContext : DbContext
{
    public BusContext(DbContextOptions<BusContext> options)
        : base(options)
    {
    }
    public DbSet<EntryModel> Entries { get; set; }
    public DbSet<LoopModel> Loops { get; set; }
    public DbSet<DriverModel> Drivers { get; set; }
    public DbSet<BusModel> Buses { get; set; }
    public DbSet<StopModel> Stops { get; set; }

    

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite($"Data Source=BusDb.db");
}
