using System.Text.Json.Serialization;

namespace Bookify.Infrastructure.Authentication;

public sealed class AuthorizationToken
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }
}