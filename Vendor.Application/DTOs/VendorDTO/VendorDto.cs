using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.Domain.Entities;

namespace Vendor.Application.DTOs.VendorDTO
{
    public class VendorDto
    {
        public string Name { get; set; }
   
        public string Email { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public ICollection<VendorMarket> VendorMarkets { get; set; }
        public bool IsApproved { get; set; }
    }
}
