using MediatR;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;


namespace Vendor.Application.Requests.Services
{
    public class AddServiceCommand:IRequest<Service>
    {
        public string Name { get; set; }
    }

    //<Summary>
    // Purpose: Handles the AddServiceCommand request to create a new service and save it to the database
    // Parameters:
    // - request (AddServiceCommand): The command request containing details to create a new service
    // Returns: Task<Service>: A task that represents the asynchronous operation, containing the created Service object
    // Exceptions: ArgumentException: Thrown if the service name is null or whitespace
    //</Summary>

    public class AddServiceCommandHandler : IRequestHandler<AddServiceCommand, Service> 
    {
        private readonly Infrastructure.Implementation.Persistence.IVendorDbContext _vendorDbContext;
        /// <summary>
        /// Initializes a new instance of the <see cref="AddServiceCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IVendorDbContext"/> instance used to interact with the database.</param>
        public AddServiceCommandHandler(Infrastructure.Implementation.Persistence.IVendorDbContext vendorDbContext)
        {
            _vendorDbContext = vendorDbContext;
        }
        public async Task<Service> Handle(AddServiceCommand request,CancellationToken cancellationToken)
        {

            // 1. Handle the incoming AddServiceCommand request to add a new service to the database.
            // 2. Check if the provided service name is null or consists only of whitespace.
            // 3. If the service name is invalid, throw an ArgumentException with a descriptive error message.
            // 4. Create a new Service entity and assign the provided name to it.
            // 5. Add the newly created Service entity to the database context.
            // 6. Save the changes to the database asynchronously using SaveChangesAsync method.
            // 7. Return the newly created and saved Service entity.

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Service name can't be null",nameof(request.Name));
            }
            var service=new Service
            {
                Name = request.Name

            };
            _vendorDbContext.Services.Add(service);
            await _vendorDbContext.SaveChangesAsync();
            return service;
            
        }


    }
}
