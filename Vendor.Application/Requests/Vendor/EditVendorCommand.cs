using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;
using FluentValidation;

namespace Vendor.Application.Requests.Vendor
{
    public record EditVendorCommandWithId : IRequest<CreateDto>
    {
        public int Id { get; init; }
        public EditVendorCommand Command { get; init; }
        public EditVendorCommandWithId(int id, EditVendorCommand command)
        {
            Id = id;
            Command = command;
        }
    }

    public record EditVendorCommand : IRequest<CreateDto>
    {
        public string Name { get; init; }
        public string City { get; init; }
        public string StateProvinceRegion { get; init; }
        public string PostalCode { get; init; }
        public string Country { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public string Website { get; init; }
        public int ServiceId { get; init; }
        public bool IsApproved { get; init; }
        public ICollection<int> MarketIds { get; init; }
    }

    //<Summary>
    // Purpose: Handles the EditVendorCommandWithId request to edit an existing vendor in the database
    // Parameters:
    // - request (EditVendorCommandWithId): The command request containing the vendor ID and details to edit an existing vendor
    // Returns: Task<CreateDto>: A task that represents the asynchronous operation, containing a CreateDto with the details of the edited vendor
    //</Summary>
    public class EditVendorHandler : IRequestHandler<EditVendorCommandWithId, CreateDto>
    {
        private readonly IVendorDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly EditVendorCommandValidator _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditVendorHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IVendorDbContext"/> instance used to interact with the database.</param>
        /// <param name="mapper">The <see cref="IMapper"/> instance used for auto mapping.</param>
        /// <param name="validator">The <see cref="EditVendorCommandValidator"/> instance used for validating the edit command.</param>
        public EditVendorHandler(IVendorDbContext dbContext, IMapper mapper, EditVendorCommandValidator validator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CreateDto> Handle(EditVendorCommandWithId request, CancellationToken cancellationToken)
        {
            /// 1. Validate the edit vendor command
            /// 2. Retrieve the existing vendor from the database
            /// 3. Map the updated properties from the command to the existing vendor
            /// 4. Update the vendor's market associations
            /// 5. Save changes to the database
            /// 6. Map the updated vendor entity to a CreateDto and return it

            var validationResult = await _validator.ValidateAsync(request.Command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingVendor = await _dbContext.Vendors
            .Include(v => v.VendorMarket)
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            if (existingVendor == null)
            {
                throw new ArgumentException($"Vendor with ID {request.Id} not found");
            }

            _mapper.Map(request.Command, existingVendor);
            UpdateVendorMarkets(existingVendor, request.Command.MarketIds);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CreateDto>(existingVendor);
        }

        /// <summary>
        /// Updates the vendor's market associations based on the new set of market IDs
        /// </summary>
        /// <param name="vendor">The vendor entity to update</param>
        /// <param name="newMarketIds">The new set of market IDs</param>
        private void UpdateVendorMarkets(Domain.Entities.Vendor vendor, ICollection<int> newMarketIds)
        {
            var existingMarketIds = vendor.VendorMarket.Select(vm => vm.MarketId).ToList();

            foreach (var vendorMarket in vendor.VendorMarket.ToList())
            {
                if (!newMarketIds.Contains(vendorMarket.MarketId))
                {
                    vendor.VendorMarket.Remove(vendorMarket);
                }
            }

            foreach (var marketId in newMarketIds)
            {
                if (!existingMarketIds.Contains(marketId))
                {
                    vendor.VendorMarket.Add(new VendorMarket { MarketId = marketId, VendorId = vendor.Id });
                }
            }
        }
    }
}