

using Vendor.Domain.Entities.Common;

namespace Vendor.Domain.Entities
{
    public class Market :BaseEntity
    {

        public ICollection<VendorMarket> VendorMarket { get; set; } 
    }
}
