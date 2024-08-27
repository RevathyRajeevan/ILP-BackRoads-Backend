using System.Text.Json.Serialization;
using Vendor.Domain.Entities.Common;


namespace Vendor.Domain.Entities
{

    public class Vendor :BaseEntity
    {

        public string City { get; set; }
        public string StateProvinceRegion { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }

        public int ServiceId { get; set; }

        [JsonIgnore]
        public Service Service { get; set; }

        public bool IsApproved { get; set; }


        public ICollection<VendorMarket> VendorMarket { get; set; }
    }

}