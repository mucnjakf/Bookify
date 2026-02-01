namespace Bookify.Infrastructure.Authentication;

internal sealed class AuthenticationOptions
{
    internal const string SectionName = "Authentication";

    internal required string Audience { get; init; }

    internal required string MetadataUrl { get; init; }

    internal required bool RequireHttpsMetadata { get; init; }

    internal required string Issuer { get; init; }
}