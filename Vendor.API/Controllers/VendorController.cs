﻿using MediatR;
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VendorDataDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<VendorDataDto>> GetVendorData([FromQuery] int id, CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetVendorByIdQuery
                {
                    Id = id
                };
                var response = await _mediator.Send(query, cancellationToken);
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VendorInfo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VendorInfo>> CreateVendor([FromBody] AddVendorCommand request, CancellationToken cancellationToken)
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


        ///<summary>
        /// Method: EditVendor
        /// Purpose: Updates an existing vendor's information
        /// Route: api/vendor/editvendor
        /// HTTP Method: PUT
        /// Response Types:
        /// - 200 OK with the updated VendorDto if successful
        /// - 400 Bad Request if validation fails or an error occurs
        /// - 404 Not Found if the vendor is not found
        ///</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VendorInfo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VendorInfo>> EditVendor(int id, [FromBody] EditVendorCommand request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var commandWithId = new EditVendorCommandWithId(id, request);
                var vendor = await _mediator.Send(commandWithId, cancellationToken);
                return Ok(vendor);
            }

            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Method: ApproveVendor
        /// Purpose: Approves an existing vendor by setting its IsApproved status to true
        /// Route: api/vendor/approvevendor
        /// HTTP Method: PATCH
        /// Response Types:
        /// - 200 OK with the approved Vendor entity if successful
        /// - 400 Bad Request if an error occurs
        /// </summary>
        [HttpPatch()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ApproveVendor([FromQuery] int id, CancellationToken cancellationToken)
        {
            try
            {
                var command = new ApproveVendorCommand
                {
                    Id = id
                };
                var result = await _mediator.Send(command, cancellationToken);

                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

    
