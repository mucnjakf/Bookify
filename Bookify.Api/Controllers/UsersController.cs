using Bookify.Application.Users.GetCurrentUser;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public sealed class UsersController(ISender sender) : ControllerBase
{
    [HttpGet("me")]
    [Authorize(Roles = Roles.Registered)]
    public async Task<ActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserQuery();

        Result<UserDto> result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }
}