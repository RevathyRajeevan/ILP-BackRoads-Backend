using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;
using Vendor.Infrastructure.Implementation.Persistence.EntityConfigurations;

public class VendorDbContext : DbContext,IVendorDbContext
{
    public VendorDbContext(DbContextOptions<VendorDbContext> options) : base(options) { }

    public DbSet<Vendor.Domain.Entities.Vendor> Vendors { get; set; }
    public DbSet<Market> Markets { get; set; }
    public DbSet<VendorMarket> VendorMarkets { get; set; }
    public DbSet<Service> Services { get; set; }

    Task<int> IVendorDbContext.SaveChangesAsync() => base.SaveChangesAsync();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("vendor");

        modelBuilder.ApplyConfiguration(new VendorEntityConfiguration());
        modelBuilder.ApplyConfiguration(new VendorMarketEntityConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
