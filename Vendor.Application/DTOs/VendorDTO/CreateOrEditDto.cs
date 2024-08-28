using Vendor.Application.DTOs.VendorDTO.BaseDTO;

namespace Vendor.Application.DTOs.VendorDTO
{
    // Represents a Data Transfer Object (DTO) for creating or returning vendor information.
    public class CreateOrEditDto : BaseDto
    {
        public string ServiceName { get; set; } // Name of the service associated with the vendor.
        public List<string> Markets { get; set; } // List of market names associated with the vendor.
    }

}
