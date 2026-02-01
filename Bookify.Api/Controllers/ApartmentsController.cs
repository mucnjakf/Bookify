using Bookify.Application.Apartments.SearchApartments;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/apartments")]
public sealed class ApartmentsController(ISender sender) : ControllerBase
{
    [HttpGet("search/start-date/{startDate}/end-date/{endDate}")]
    public async Task<ActionResult> SearchApartments(
        [FromRoute] DateOnly startDate,
        [FromRoute] DateOnly endDate,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<ApartmentDto>> result = await sender
            .Send(new SearchApartmentsQuery(startDate, endDate), cancellationToken);

        return Ok(result.Value);
    }
}