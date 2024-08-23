using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Requests.Markets;
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

        //<Summary>
        // Method: AddMarket
        // Purpose: Creates a new market
        // Route: api/service/addmarket
        // HTTP Method: POST
        // Request:
        // - Body: AddMarketCommand (Contains the details of the market to be created)
        // Response Types:
        // - 201 Created with the created Market object if successful
        // - 400 Bad Request if the request data is invalid or an error occurs
        // - 204 No Content if no content is returned (not used in this case, but included in response types)
        //</Summary>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Market))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Market>> AddMarket([FromBody] AddMarketCommand request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        //<Summary>
        // Method: GetMarkets
        // Purpose: Retrieves a list of markets
        // Route: api/service/getmarkets
        // HTTP Method: GET
        // Response Types:
        // - 200 OK with a list of Market objects if successful and markets are found
        // - 400 Bad Request if an error occurs
        //</Summary>


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Market>>> GetMarkets(CancellationToken cancellationToken)
        {
            var markets = await _mediator.Send(new GetMarketQuery(), cancellationToken);
            if (markets == null)
            {
                return NotFound();
            }
            return Ok(markets);
        }
    }

}
