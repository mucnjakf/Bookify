namespace Bookify.Api.Controllers.Requests;

public sealed record LoginUserRequest(string Email, string Password);