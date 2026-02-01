using System.Text.Json.Serialization;

namespace Bookify.Infrastructure.Authentication;

internal sealed class AuthorizationToken
{
    [JsonPropertyName("access_token")]
    internal required string AccessToken { get; init; }
}