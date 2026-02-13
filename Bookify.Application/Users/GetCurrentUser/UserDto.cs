namespace Bookify.Application.Users.GetCurrentUser;

public sealed record UserDto
{
    public Guid Id { get; init; }

    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }
}