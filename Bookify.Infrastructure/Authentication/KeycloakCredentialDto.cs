namespace Bookify.Infrastructure.Authentication;

internal sealed class KeycloakCredentialDto
{
    internal string? Value { get; set; }

    internal bool Temporary { get; set; }

    internal string? Type { get; set; }
}