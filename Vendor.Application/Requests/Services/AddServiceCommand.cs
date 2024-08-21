using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.Domain.Entities;


namespace Vendor.Application.Requests.Services
{
    public class AddServiceCommand:IRequest<Service>
    {
        public string Name { get; set; }
    }
    public class AddServiceCommandHandler : IRequestHandler<AddServiceCommand, Service> 
    {
        private readonly VendorDbContext _vendorDbContext;
        public AddServiceCommandHandler(VendorDbContext vendorDbContext)
        {
            _vendorDbContext = vendorDbContext;
        }
        public async Task<Service> Handle(AddServiceCommand request,CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Service name can't be null",nameof(request.Name));
            }
            var service=new Service
            {
                Name = request.Name

            };
            _vendorDbContext.Services.Add(service);
            await _vendorDbContext.SaveChangesAsync(cancellationToken);
            return service;
            
        }


    }
}
