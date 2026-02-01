using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Authentication;

internal sealed class KeycloakUserDto
{
    internal Dictionary<string, string> Access { get; set; } = [];

    internal Dictionary<string, List<string>> Attributes { get; set; } = [];

    internal Dictionary<string, string> ClientRoles { get; set; } = [];

    internal long? CreatedTimestamp { get; set; }

    internal KeycloakCredentialDto[] Credentials { get; set; } = [];

    internal string[] DisabledCredentialTypes { get; set; } = [];

    internal string? Email { get; set; }

    internal bool? EmailVerified { get; set; }

    internal bool? Enabled { get; set; }

    internal string? FederationLink { get;  set; }

    internal string? Id { get; set; }

    internal string[] Groups { get; set; } = [];

    internal string? FirstName { get; set; }

    internal string? LastName { get; set; }

    internal int? NotBefore { get; set; }

    internal string? Origin { get; set; }

    internal string[] RealmRoles { get; set; } = [];

    internal string[] RequiredActions { get; set; } = [];

    internal string? Self { get; set; }

    internal string? ServiceAccountClientId { get; set; }

    internal string? Username { get; set; }

    internal static KeycloakUserDto FromUser(User user) => new()
    {
        FirstName = user.FirstName.Value,
        LastName = user.LastName.Value,
        Email = user.Email.Value,
        Username = user.Email.Value,
        Enabled = true,
        EmailVerified = true,
        CreatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        Attributes = new Dictionary<string, List<string>>(),
        RequiredActions = []
    };
}