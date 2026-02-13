using Bookify.Api.Controllers.Requests;
using Bookify.Application.Users.LoginUser;
using Bookify.Application.Users.RegisterUser;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/auth")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        Result<Guid> result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest();
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> LoginUser(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(request.Email, request.Password);

        Result<TokenDto> result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }
}