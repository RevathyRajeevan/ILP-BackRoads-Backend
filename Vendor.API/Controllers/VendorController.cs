using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vendor.Application.DTOs.VendorDTO;
using Vendor.Application.Requests.Vendor;

namespace Vendor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VendorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //<Summary>
        // Method: GetVendors
        // Purpose: Retrieves all vendors from the database
        // Route: api/vendor/getallvendors
        // HTTP Method: GET
        // Response Types:
        // - 200 OK with a list of VendorDto if successful
        // - 400 Bad Request if an error occurs
        //</Summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VendorDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<VendorDto>>> GetVendors(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(new GetVendorsQuery(), cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        //<Summary>
        // Method: GetVendorById
        // Purpose: Retrieves vendor details by Id from the database
        // Route: api/vendor/GetVendorById/{id}
        // HTTP Method: GET
        // Response Types:
        // - 200 OK with VendorDto if successful
        // - 400 Bad Request if an error occurs
        //</Summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VendorByIdDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<VendorByIdDto>> GetVendorById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(new GetVendorByIdQuery(id), cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        //<Summary>
        // Method: CreateVendor
        // Purpose: Creates a new vendor
        // Route: api/vendor/createvendor
        // HTTP Method: POST
        // Response Types:
        // - 201 Created with the created VendorDto if successful
        // - 400 Bad Request if validation fails or an error occurs
        //</Summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateDto>> CreateVendor([FromBody] AddVendorCommand request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var vendor = await _mediator.Send(request, cancellationToken);
                return CreatedAtAction(nameof(GetVendors), new { id = vendor.Id }, vendor); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
