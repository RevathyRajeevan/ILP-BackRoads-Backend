using Vendor.Domain.Entities.Common;

namespace Vendor.Domain.Entities
{
    public class Service :BaseEntity
    {

        public ICollection<Vendor> Vendors { get; set; } 

    }

}
