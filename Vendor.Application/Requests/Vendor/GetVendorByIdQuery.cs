using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Infrastructure.Implementation.Persistence;
namespace Vendor.Application.Requests.Vendor
{
    // Query class to request a vendor's details by its ID.
    public class GetVendorByIdQuery : IRequest<VendorDataDto>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Handles the request to get vendor details by vendor ID.
    /// - Fetches the vendor by ID from the database.
    /// - Includes related data such as Service and VendorMarket entities, as well as Market entities within VendorMarket.
    /// - Maps the vendor entity to a VendorDataDto object.
    /// - Maps the associated markets to the Markets property in the VendorDataDto.
    /// - Returns the populated VendorDataDto object.
    /// </summary>
    public class GetVendorByIdHandler : IRequestHandler<GetVendorByIdQuery, VendorDataDto>
    {
        private readonly IVendorDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetVendorByIdHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IVendorDbContext"/> instance used to interact with the database.</param>
        /// <param name="mapper">The <see cref="IMapper"/> instance used for auto mapping.</param>
        public GetVendorByIdHandler(IVendorDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<VendorDataDto> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
        {
            // Fetch the vendor by ID from the database, including their related Service and VendorMarket entities.
            // ThenInclude is used to further load the Market entities associated with each VendorMarket.
            // Map the Vendor entity to VendorDataDto object using AutoMapper.
            // The markets are selected from the VendorMarket collection for the vendor and mapped to MarketDto objects.
            // Assign the list of MarketDto objects to the Markets property of the VendorDataDto.
            // Return the VendorDataDto object, enriched with its associated markets.

            var vendor = await _dbContext.Vendors
                .Include(v => v.Service)
                .Include(v => v.VendorMarket)
                    .ThenInclude(vm => vm.Market)
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            if (vendor == null)
            {
                return null;
            }

            var VendorDataDto = _mapper.Map<VendorDataDto>(vendor);

            return VendorDataDto;
        }
    }
}
