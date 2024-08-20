using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.Domain.Entities;

namespace Vendor.Infrastructure.Implementation.Persistence.EntityConfigurations
{
    public class VendorMarketEntityConfiguration: IEntityTypeConfiguration<VendorMarket>
    {
        public void Configure(EntityTypeBuilder<VendorMarket> modelBuilder)
        {
            modelBuilder.HasKey(vm => new { vm.VendorId, vm.MarketId });

            modelBuilder.HasOne(vm => vm.Vendor)
                .WithMany(v => v.VendorMarket)
                .HasForeignKey(vm => vm.VendorId);

            modelBuilder.HasOne(vm => vm.Market)
                .WithMany(m => m.VendorMarket)
                .HasForeignKey(vm => vm.MarketId);

        }
    }
    
}
