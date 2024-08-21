using Microsoft.AspNetCore.Mvc;
using MediatR;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Application.Requests.Vendor;
namespace Vendor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class VendorController: ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Vendor.Domain.Entities.Vendor>>> GetAllVendors(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetAllVendorsQuery(), cancellationToken);
            return Ok(response);
        }
        }

    }
