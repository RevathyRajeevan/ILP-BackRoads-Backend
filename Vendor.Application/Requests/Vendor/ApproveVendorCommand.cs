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

    public class ApproveVendorCommandHandler : IRequestHandler<ApproveVendorCommand, Domain.Entities.Vendor>
    {
        private readonly IVendorDbContext _dbcontext;

        public ApproveVendorCommandHandler(IVendorDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<Domain.Entities.Vendor> Handle(ApproveVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _dbcontext.Vendors.FindAsync(request.Id);

            if (vendor == null)
            {
                return null; // Or throw an appropriate exception
            }

            vendor.IsApproved = true;
            await _dbcontext.SaveChangesAsync();

            return vendor;
        }
    }


}
