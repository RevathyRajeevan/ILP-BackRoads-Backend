using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence.EntityConfigurations;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

    public DbSet<Vendors> Vendors { get; set; }
    public DbSet<Markets> Markets { get; set; }
    public DbSet<VendorMarkets> VendorMarkets { get; set; }
    public DbSet<Services> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VendorEntityConfiguration());
        modelBuilder.ApplyConfiguration(new VendorMarketEntityConfiguration());


        base.OnModelCreating(modelBuilder);
    }
}
