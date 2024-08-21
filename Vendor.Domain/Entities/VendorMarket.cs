namespace Vendor.Domain.Entities
{
    public class VendorMarket
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public int MarketId { get; set; }
        public Market Market { get; set; }

    }
}
