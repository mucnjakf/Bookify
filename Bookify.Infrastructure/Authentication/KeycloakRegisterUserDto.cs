using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Authentication;

internal sealed class KeycloakRegisterUserDto
{
    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool? Enabled { get; set; }

    public bool? EmailVerified { get; set; }

    public List<KeycloakCredentialDto> Credentials { get; set; } = [];

    public static KeycloakRegisterUserDto FromUser(User user) =>
        new()
        {
            Username = user.Email.Value,
            Email = user.Email.Value,
            FirstName = user.FirstName.Value,
            LastName = user.LastName.Value,
            Enabled = true,
            EmailVerified = true
        };
}