using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Vendor.Infrastructure.Implementation.Persistence.EntityConfigurations
{
    public class VendorEntityConfiguration: IEntityTypeConfiguration<Vendor.Domain.Entities.Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor.Domain.Entities.Vendor> modelBuilder)
        {
            modelBuilder.HasOne(v => v.Service)
            .WithMany(s => s.Vendors)
            .HasForeignKey(v => v.ServiceId);
        }
        
    }
}
