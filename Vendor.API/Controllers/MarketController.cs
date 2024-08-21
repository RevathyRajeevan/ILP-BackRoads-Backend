using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Requests.Markets;
using Vendor.Application.Requests.Vendor;
using Vendor.Domain.Entities;

namespace Vendor.API.Controllers
{

        [ApiController]
        [Route("api/[controller]/[Action]")]
        public class MarketController : ControllerBase
        {
            private readonly IMediator _mediator;

            public MarketController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpPost]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Market>> AddMarket([FromBody] AddMarketCommand request, CancellationToken cancellationToken)
            {
                        var response = await _mediator.Send(request, cancellationToken);
                        return Ok(response);
            }
        }
    
}
