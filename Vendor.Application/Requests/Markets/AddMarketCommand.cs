using MediatR;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;

namespace Vendor.Application.Requests.Markets
{
    public class AddMarketCommand: IRequest<Market>
    {
        public string Name { get; set; }
    }

    //<Summary>
    // Purpose: Handles the AddMarketCommand request to create a new market and save it to the database
    // Parameters:
    // - request (AddMarketCommand): The command request containing details to create a new market
    // Returns: Task<Market>: A task that represents the asynchronous operation, containing the created Market object
    // Exceptions: ArgumentException: Thrown if the market name is null or whitespace
    //</Summary>

    public class AddMarketCommandHandler : IRequestHandler<AddMarketCommand, Market>
    {
        private readonly IVendorDbContext _dbContext;
        private readonly AddMarketCommandValidator _validator;
        /// <summary>
        /// Initializes a new instance of the <see cref="AddMarketCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IVendorDbContext"/> instance used to interact with the database.</param>
        public AddMarketCommandHandler(IVendorDbContext dbContext , AddMarketCommandValidator validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }
        public async Task<Market> Handle(AddMarketCommand request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage;
                throw new ArgumentException(errorMessage);
            }

            var market = new Market
            {
                Name = request.Name,
            };

            _dbContext.Markets.Add(market);
            await _dbContext.SaveChangesAsync();

            return market;
        }
    }

}

