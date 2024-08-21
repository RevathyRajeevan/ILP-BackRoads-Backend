using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Requests.Services;
using Vendor.Domain.Entities;

namespace Vendor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ServiceController
    {
        private readonly IMediator _mediator;
        public ServiceController(IMediator mediator) 
        {
            _mediator = mediator;
        
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Service>> AddService([FromBody] AddServiceCommand request,CancellationToken cancellationToken)
        {
            var response=await _mediator.Send(request,cancellationToken);
            return response;
        }

    }
}
