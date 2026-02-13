using System.Text.Json.Serialization;

namespace Bookify.Infrastructure.Authentication;

internal sealed class AuthorizationTokenDto
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }
}