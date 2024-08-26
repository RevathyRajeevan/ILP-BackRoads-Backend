using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Infrastructure.Implementation.Persistence;
namespace Vendor.Application.Requests.Vendor
{
    // Query class to request a vendor's details by its ID.
    public class GetVendorByIdQuery : IRequest<VendorByIdDto>
    {
        public int Id { get; set; }
        // Constructor to initialize the query with the vendor ID.
        public GetVendorByIdQuery(int id)
        {
            Id = id;
        }
    }

    // Handles the GetVendorByIdQuery to return vendor details by vendor ID.
    public class GetVendorByIdHandler : IRequestHandler<GetVendorByIdQuery, VendorByIdDto>
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

        /// <summary>
        /// Handles the request to get vendor details by vendor ID.
        /// - Fetches the vendor by ID from the database.
        /// - Includes related data such as Service and VendorMarket entities, as well as Market entities within VendorMarket.
        /// - Maps the vendor entity to a VendorByIdDto object.
        /// - Maps the associated markets to the Markets property in the VendorByIdDto.
        /// - Returns the populated VendorByIdDto object.
        /// </summary>
        /// <param name="request">The <see cref="GetVendorByIdQuery"/> containing the vendor ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="VendorByIdDto"/> object containing vendor details, or null if not found.</returns>
        public async Task<VendorByIdDto> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
        {
            var vendor = await _dbContext.Vendors
                .Include(v => v.Service)
                .Include(v => v.VendorMarket)
                    .ThenInclude(vm => vm.Market)
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            if (vendor == null)
            {
                return null;
            }

            var VendorByIdDto = _mapper.Map<VendorByIdDto>(vendor);

            VendorByIdDto.Markets = _mapper.Map<List<MarketDto>>(vendor.VendorMarket.Select(vm => vm.Market).ToList());

            return VendorByIdDto;
        }
    }
}
