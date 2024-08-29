using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;

namespace Vendor.Application.Requests.Vendor
{
    public record AddVendorCommand(
       string Name,
       string City,
       string StateProvinceRegion,
       string PostalCode,
       string Country,
       string Email,
       string Phone,
       string Website,
       int ServiceId,
       bool IsApproved,
       ICollection<int> MarketIds
       ) : IRequest<VendorInfo>;

    //<Summary>
    // Purpose: Handles the AddVendorCommand request to create a new vendor in the database
    // Parameters:
    // - request (AddVendorCommand): The command request containing details to create a new vendor
    // Returns:Task<CreateDto>: A task that represents the asynchronous operation, containing a CreateDto with the details of the created vendor
    //</Summary>
    public class CreateVendorHandler : IRequestHandler<AddVendorCommand, VendorInfo>
    {
        private readonly IVendorDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AddVendorCommandValidator _validator;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVendorHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IVendorDbContext"/> instance used to interact with the database.</param>
        /// <param name="mapper">The <see cref="IMapper"/> instance used for auto mapping.</param>
        public CreateVendorHandler(IVendorDbContext dbContext, IMapper mapper,AddVendorCommandValidator validator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;

        }

        public async Task<VendorInfo> Handle(AddVendorCommand request, CancellationToken cancellationToken)
        {
            /// 1. Maps the <see cref="AddVendorCommand"/> to a <see cref="Domain.Entities.Vendor"/> entity using AutoMapper.
            /// 2. Creates a list of <see cref="VendorMarket"/> associations based on the provided market IDs in the request.
            /// 3. Adds the newly created vendor entity to the database context.
            /// 4. Saves changes to the database asynchronously.
            /// 5. Maps the saved vendor entity to a <see cref="CreateDto"/> and returns it.
            /// 
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage;
                throw new ArgumentException(errorMessage);
            }

            var vendorExists = await _dbContext.Vendors
                .AnyAsync(v => v.Name == request.Name, cancellationToken);

            if (vendorExists)
            {
                throw new ArgumentException("Vendor name must be unique.");
            }

            var vendor = _mapper.Map<Domain.Entities.Vendor>(request);

            vendor.VendorMarket = request.MarketIds.Select(id => new VendorMarket { MarketId = id }).ToList();

            _dbContext.Vendors.Add(vendor);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<VendorInfo>(vendor);
        }
    }

}
