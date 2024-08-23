using Vendor.Domain.Entities;

namespace Vendor.Application.DTOs.VendorDTO
{
    // Represents a Data Transfer Object (DTO) for a Vendor with its details.
    public class VendorDto
    {
        public int Id { get; set; } // Unique identifier for the vendor.
        public string Name { get; set; } // Name of the vendor.
        public string Email { get; set; } // Email address of the vendor.
        public ServiceDto Service { get; set; } // Associated service information for the vendor.
        public List<MarketDto> Markets { get; set; } // List of markets associated with the vendor.
        public bool IsApproved { get; set; } // Indicates if the vendor is approved.
    }

    // Represents a Data Transfer Object (DTO) for a Service with its details.
    public class ServiceDto
    {
        public int Id { get; set; } // Unique identifier for the service.
        public string Name { get; set; } // Name of the service.
    }

    // Represents a Data Transfer Object (DTO) for a Market with its details.
    public class MarketDto
    {
        public int Id { get; set; } // Unique identifier for the market.
        public string Name { get; set; } // Name of the market.
    }


}
