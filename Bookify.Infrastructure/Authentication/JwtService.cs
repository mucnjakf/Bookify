using System.Net.Http.Json;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Abstractions;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authentication;

internal sealed class JwtService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions) : IJwtService
{
    private static readonly Error AuthenticationFailed = new(
        "Keycloak.Authentication",
        "Failed to acquire access token due to authentication failure");

    public async Task<Result<string>> GetTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", keycloakOptions.Value.AuthClientId),
                new("client_secret", keycloakOptions.Value.AuthClientSecret),
                new("scope", "openid email"),
                new("grant_type", "password"),
                new("username", email),
                new("password", password)
            };

            var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

            HttpResponseMessage httpResponseMessage = await httpClient
                .PostAsync(string.Empty, authorizationRequestContent, cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            var authorizationToken = await httpResponseMessage.Content
                .ReadFromJsonAsync<AuthorizationTokenDto>(cancellationToken);

            return authorizationToken is null
                ? Result.Failure<string>(AuthenticationFailed)
                : Result.Success(authorizationToken.AccessToken);
        }
        catch (HttpRequestException httpRequestException)
        {
            return Result.Failure<string>(AuthenticationFailed);
        }
    }
}