using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Infrastructure.Implementation.Persistence;

namespace Vendor.Application.Requests.Vendor
{
    public class ApproveVendorCommand : IRequest<Domain.Entities.Vendor>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Purpose: Handles the ApproveVendorCommand request to approve an existing vendor in the database.
    /// Parameters:
    /// - request (ApproveVendorCommand): The command request containing the ID of the vendor to be approved.
    /// Returns: Task<Vendor>: A task that represents the asynchronous operation, containing the approved Vendor entity or null if the vendor is not found.
    /// </summary>
    public class ApproveVendorCommandHandler : IRequestHandler<ApproveVendorCommand, Domain.Entities.Vendor>
    {
        private readonly IVendorDbContext _dbcontext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApproveVendorCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The <see cref="IVendorDbContext"/> instance used to interact with the database.</param>
        public ApproveVendorCommandHandler(IVendorDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<Domain.Entities.Vendor> Handle(ApproveVendorCommand request, CancellationToken cancellationToken)
        {

            /// 1. Retrieves the <see cref="Domain.Entities.Vendor"/> entity from the database using the provided <see cref="ApproveVendorCommand.Id"/>.
            /// 2. Checks if the vendor exists; if not, returns null.
            /// 3. Sets the <see cref="Domain.Entities.Vendor.IsApproved"/> property to true.
            /// 4. Saves the updated vendor entity to the database asynchronously.
            /// 5. Returns the approved <see cref="Domain.Entities.Vendor"/> entity.
            var vendor = await _dbcontext.Vendors.FindAsync(request.Id);

            if (vendor == null)
            {
                return null; 
            }

            vendor.IsApproved = true;
            await _dbcontext.SaveChangesAsync();

            return vendor;
        }
    }
}
