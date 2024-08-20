using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Vendor.Domain.Entities
{
    public class Markets
    {
       


        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<VendorMarkets> VendorMarkets { get; set; } = new List<VendorMarkets>();
    }
}
