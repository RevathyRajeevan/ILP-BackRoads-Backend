using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;

namespace Vendor.Application.Requests.Markets
{
    public class GetMarketQuery:IRequest<IEnumerable<Market>>
    {
        public int Id { get; set; }

    }
    //<Summary>
    // Purpose: Handles the GetMarketQuery request to retrieve a list of markets from the database
    // Parameters:
    // - request (GetMarketQuery): The query request to fetch markets
    // Returns: containing a list of Market objects
    //</Summary>
    public class GetMarketQueryHandler : IRequestHandler<GetMarketQuery, IEnumerable<Market>>
    {
        private Infrastructure.Implementation.Persistence.IVendorDbContext _vendorDbContext;
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMarketQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IVendorDbContext"/> instance used to interact with the database.</param>
        public GetMarketQueryHandler(Infrastructure.Implementation.Persistence.IVendorDbContext vendorDbContext)
        {
            _vendorDbContext = vendorDbContext;
        }
        public async Task<IEnumerable<Market>> Handle(GetMarketQuery request, CancellationToken cancellationToken)
        {
            // 1. Handle the incoming GetMarketQuery request to retrieve a list of markets from the database.
            // 2. Asynchronously fetch all Market entities from the database using ToListAsync method with the provided cancellation token.
            // 3. Return the list of Market entities.

            var markets = await _vendorDbContext.Markets.ToListAsync(cancellationToken);
            return markets;
        }
    }
}
