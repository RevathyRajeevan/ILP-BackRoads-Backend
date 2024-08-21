using Vendor.Domain.Entities;

public class Vendors
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string StateProvinceRegion { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Website { get; set; }

    public int ServiceId { get; set; }
    public Service Service { get; set; } 

    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<VendorMarket> VendorMarket { get; set; } = new List<VendorMarket>();
}
