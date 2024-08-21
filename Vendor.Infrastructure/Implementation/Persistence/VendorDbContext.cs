using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence.EntityConfigurations;

public class VendorDbContext : DbContext
{
    public VendorDbContext(DbContextOptions<VendorDbContext> options) : base(options) { }

    public DbSet<Vendors> Vendors { get; set; }
    public DbSet<Market> Markets { get; set; }
    public DbSet<VendorMarket> VendorMarkets { get; set; }
    public DbSet<Service> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("vendor");

        modelBuilder.ApplyConfiguration(new VendorEntityConfiguration());
        modelBuilder.ApplyConfiguration(new VendorMarketEntityConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
