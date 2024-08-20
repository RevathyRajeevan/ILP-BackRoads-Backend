using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Entities;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

    public DbSet<Vendors> Vendors { get; set; }
    public DbSet<Markets> Markets { get; set; }
    public DbSet<VendorMarkets> VendorMarkets { get; set; }
    public DbSet<Services> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VendorMarkets>()
            .HasKey(vm => new { vm.VendorId, vm.MarketId });

        modelBuilder.Entity<VendorMarkets>()
            .HasOne(vm => vm.Vendor)
            .WithMany(v => v.VendorMarkets)
            .HasForeignKey(vm => vm.VendorId);

        modelBuilder.Entity<VendorMarkets>()
            .HasOne(vm => vm.Market)
            .WithMany(m => m.VendorMarkets)
            .HasForeignKey(vm => vm.MarketId);

        modelBuilder.Entity<Vendors>()
            .HasOne(v => v.Service)
            .WithMany(s => s.Vendors)
            .HasForeignKey(v => v.ServiceId);

        base.OnModelCreating(modelBuilder);
    }
}
