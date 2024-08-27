using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Entities;

namespace Vendor.Infrastructure.Implementation.Persistence
{
    public interface IVendorDbContext
    {
        public DbSet<Vendor.Domain.Entities.Vendor> Vendors { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<VendorMarket> VendorMarkets { get; set; }
        public DbSet<Service> Services { get; set; }

        Task<int> SaveChangesAsync();
    }
}
