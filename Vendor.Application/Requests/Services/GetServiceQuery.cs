using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Entities;
using Vendor.Infrastructure.Implementation.Persistence;

namespace Vendor.Application.Requests.Services
{
    public class GetServiceQuery:IRequest<IEnumerable<Service>>
    {
        public int Id { get; set; }
       
    }

    //<Summary>
    // Purpose: Handles the GetServiceQuery request to retrieve a list of services from the database
    // Parameters:
    // - request (GetServiceQuery): The query request to fetch services
    // Returns: containing a list of Service objects
    //</Summary>

    public class GetServiceQueryHandler:IRequestHandler<GetServiceQuery,IEnumerable<Service>>
    {
        private Infrastructure.Implementation.Persistence.IVendorDbContext _vendorDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetServiceQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IVendorDbContext"/> instance used to interact with the database.</param>
        public GetServiceQueryHandler(Infrastructure.Implementation.Persistence.IVendorDbContext vendorDbContext)
        {
            _vendorDbContext=vendorDbContext;
        }
        public async Task<IEnumerable<Service>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
        {

            // 1. Handle  request to retrieve services from the database.
            // 2. fetch the list of all Service entities from the database using Entity Framework's ToListAsync method.
            // 3. Return the list of Service entities fetched from the database.

            var services = await _vendorDbContext.Services.ToListAsync(cancellationToken);
            return services;
        }
    }
}
