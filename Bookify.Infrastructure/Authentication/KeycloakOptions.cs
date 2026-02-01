namespace Bookify.Infrastructure.Authentication;

internal sealed class KeycloakOptions
{
    internal const string SectionName = "Keycloak";

    internal required string AdminUrl { get; init; }

    internal required string TokenUrl { get; init; }

    internal required string AdminClientId { get; init; }

    internal required string AdminClientSecret { get; init; }

    internal required string AuthClientId { get; init; }

    internal required string AuthClientSecret { get; init; }
}