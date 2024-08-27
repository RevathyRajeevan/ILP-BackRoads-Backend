using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendor.Application.DTOs.VendorDTO.BaseDTO
{
    public class BaseDto
    {
        public int Id { get; set; } // Unique identifier for the vendor.
        public string Name { get; set; } // Name of the vendor.
        public string StateProvinceRegion { get; set; } // State, province, or region of the vendor.
        public string Country { get; set; } // Country where the vendor is based.
        public string Email { get; set; } // Email address of the vendor.
        public string Phone { get; set; } // Phone number of the vendor.
        public string Website { get; set; } // Website URL for the vendor.
        public bool IsApproved { get; set; } // Indicates if the vendor is approved.
    }
}
