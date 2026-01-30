using Bookify.Application.Apartments.SearchApartments;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Apartments;

[ApiController]
[Route("api/apartments")]
public sealed class ApartmentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> SearchApartments(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<ApartmentResponse>> result =
            await sender.Send(new SearchApartmentsQuery(startDate, endDate), cancellationToken);

        return Ok(result.Value);
    }
}