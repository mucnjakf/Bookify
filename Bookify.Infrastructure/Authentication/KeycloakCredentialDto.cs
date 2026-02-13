namespace Bookify.Infrastructure.Authentication;

internal sealed class KeycloakCredentialDto
{
    public string? Value { get; set; }

    public bool Temporary { get; set; }

    public string? Type { get; set; }
}