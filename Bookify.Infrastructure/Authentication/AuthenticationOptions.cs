namespace Bookify.Infrastructure.Authentication;

public sealed class AuthenticationOptions
{
    internal const string SectionName = "Authentication";

    public required string Audience { get; init; }

    public required string MetadataUrl { get; init; }

    public required bool RequireHttpsMetadata { get; init; }

    public required string Issuer { get; init; }
}