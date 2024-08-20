using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.Domain.Entities;

namespace Vendor.Domain.Entities
{
    public class VendorMarkets
    {
        public int VendorId { get; set; }
        public Vendors Vendor { get; set; }

        public int MarketId { get; set; }
        public Markets Market { get; set; }

    }
}
