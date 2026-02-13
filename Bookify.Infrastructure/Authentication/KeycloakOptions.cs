namespace Bookify.Infrastructure.Authentication;

public sealed class KeycloakOptions
{
    internal const string SectionName = "Keycloak";

    public required string AdminUrl { get; init; }

    public required string TokenUrl { get; init; }

    public required string AdminClientId { get; init; }

    public required string AdminClientSecret { get; init; }

    public required string AuthClientId { get; init; }

    public required string AuthClientSecret { get; init; }
}