using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Vendor.Domain.Entities;

namespace Vendor.Infrastructure.Implementation.Persistence.EntityConfigurations
{
    public class VendorEntityConfiguration: IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> modelBuilder)
        {
            modelBuilder.HasOne(v => v.Service)
            .WithMany(s => s.Vendor)
            .HasForeignKey(v => v.ServiceId);
        }
        
    }
}
