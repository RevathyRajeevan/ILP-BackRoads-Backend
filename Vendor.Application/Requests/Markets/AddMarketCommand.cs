using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vendor.Application.Requests.Vendor;
using Vendor.Domain.Entities;

namespace Vendor.Application.Requests.Markets
{
    public class AddMarketCommand: IRequest<Market>
    {
        public string Name { get; set; }
    }
    public class AddMarketCommandHandler : IRequestHandler<AddMarketCommand, Market>
    {
        private readonly VendorDbContext _dbContext;

        public AddMarketCommandHandler(VendorDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Market> Handle(AddMarketCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Market name cannot be null or empty.", nameof(request.Name));
            }

            var market = new Market
            {
                Name = request.Name,
            };

            _dbContext.Markets.Add(market);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return market;
        }
    }

}

