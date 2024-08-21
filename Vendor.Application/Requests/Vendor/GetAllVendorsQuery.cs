using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Vendor.Application.Requests.Vendor
{
    public class GetAllVendorsQuery : IRequest<List<Domain.Entities.Vendor>>
    {
        public int Id { get; set; } 
    }
    public class GetAllVendorsHandler : IRequestHandler<GetAllVendorsQuery, List<Domain.Entities.Vendor>>
    {
        private readonly VendorDbContext _dbContext;

        public GetAllVendorsHandler(VendorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Domain.Entities.Vendor>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Vendors
             .Include(v => v.Service)
             .Include(v => v.VendorMarket)
             .ToListAsync(cancellationToken);
        }
    }
}
