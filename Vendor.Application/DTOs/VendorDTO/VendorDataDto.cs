using Vendor.Application.DTOs.VendorDTO.BaseDTO;

namespace Vendor.Application.DTOs.VendorDTO
{
    public class VendorDataDto : BaseDto
    {
        public ServiceDto Service { get; set; } // Associated service information for the vendor.
        public List<MarketDto> Markets { get; set; } // List of markets associated with the vendor.
    }
}
