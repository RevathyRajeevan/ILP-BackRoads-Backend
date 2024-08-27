using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Infrastructure.Implementation.Persistence;
namespace Vendor.Application.Requests.Vendor
{

    // Query class to request a list of all vendors
    public class GetVendorsQuery : IRequest<IEnumerable<VendorDto>>
    {
        public int Id { get; set; } 
    }


    //<Summary>
    //  Processes the GetAllVendorsQuery and returns a list of VendorDto
    // - Retrieves all vendor entities from the database.
    // - Includes related data such as Service and VendorMarket, as well as Market entities within VendorMarket.
    // - Maps the retrieved vendor entities to VendorDto objects.
    // - Creates a dictionary mapping each vendor's Id to its associated list of MarketDto objects.
    // - Populates each VendorDto with the corresponding list of markets.
    // - Returns the list of VendorDto objects, now including their associated markets.
    //</Summary>
    public class GetVendorsHandler : IRequestHandler<GetVendorsQuery, IEnumerable<VendorDto>>
    {
        private readonly IVendorDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetVendorsHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IVendorDbContext"/> instance used to interact with the database.</param>
        /// <param name="mapper">The <see cref="IMapper"/> instance used for auto mapping.</param>
        public GetVendorsHandler(IVendorDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VendorDto>> Handle(GetVendorsQuery request, CancellationToken cancellationToken)
        {
            // Fetch the list of vendors from the database, including their related Service and VendorMarket entities.
            // ThenInclude is used to further load the Market entities associated with each VendorMarket.
            // Map the list of Vendor entities to a list of VendorDto objects using AutoMapper.
            // Create a dictionary that maps each vendor's ID to a list of MarketDto objects.
            // The markets are selected from the VendorMarket collection for each vendor and mapped to MarketDto objects.
            // Iterate through each VendorDto in the list.
            // Try to get the markets associated with the current vendor from the vendorMarkets dictionary.
            // If markets are found, assign them to the Markets property of the VendorDto.
            // Return the list of VendorDto objects, each enriched with its associated markets.


            var vendors = await _dbContext.Vendors
                .Include(v => v.Service)
                .Include(v => v.VendorMarket)
                    .ThenInclude(vm => vm.Market)
                .ToListAsync(cancellationToken);

            var vendorDetails = _mapper.Map<IEnumerable<VendorDto>>(vendors);

            return vendorDetails;
        }
    }
}
