using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Requests.Services;
using Vendor.Domain.Entities;

namespace Vendor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ServiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //<Summary>
        // Method: AddService
        // Purpose: Creates a new service
        // Route: api/vendor/addservice
        // HTTP Method: POST
        // Request:
        // - Body: AddServiceCommand (contains details of the service to be created)
        // Response Types:
        // - 201 Created with the created Service object if successful
        // - 204 No Content if no content is returned (less common for POST)
        // - 400 Bad Request if the request is invalid or an error occurs
        //</Summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Service))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Service>> AddService([FromBody] AddServiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(request, cancellationToken);
                return CreatedAtAction(nameof(GetService), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //<Summary>
        // Method: GetService
        // Purpose: Retrieves a service by its ID
        // HTTP Method: GET
        // Request: GetServiceQuery
        // Response Types:
        // - 200 OK with the Service object if the service contains
        // - 404 Not Found if the service is null
        //</Summary>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Service))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Service>> GetService( CancellationToken cancellationToken)
        {
            var service = await _mediator.Send(new GetServiceQuery(), cancellationToken);

            if (service == null)
            {
                return NotFound();
            }
            return Ok(service);
        }
    }
}
