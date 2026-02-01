using Bookify.Api.Controllers.Requests;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/bookings")]
public sealed class BookingsController(ISender sender) : ControllerBase
{
    [HttpGet("{bookingId:guid}")]
    public async Task<ActionResult> GetBooking(
        [FromRoute] Guid bookingId,
        CancellationToken cancellationToken)
    {
        Result<BookingDto> result = await sender.Send(new GetBookingQuery(bookingId), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> ReserveBooking(
        [FromBody] ReserveBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        Result<Guid> result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
    }
}