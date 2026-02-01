using System.Net.Http.Json;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Authentication;

internal sealed class AuthenticationService(HttpClient httpClient) : IAuthenticationService
{
    private const string PasswordCredentialType = "password";

    public async Task<string> RegisterAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default)
    {
        KeycloakUserDto keycloakUserDto = KeycloakUserDto.FromUser(user);

        keycloakUserDto.Credentials =
        [
            new KeycloakCredentialDto
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        ];

        HttpResponseMessage? response = await httpClient
            .PostAsJsonAsync("users", keycloakUserDto, cancellationToken);

        return ExtractIdentityIdFromLocationHeader(response);
    }

    private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage response)
    {
        string? locationHeader = response.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header cannot be null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf("users/", StringComparison.InvariantCultureIgnoreCase);

        string userIdentityId = locationHeader[(userSegmentValueIndex + "users/".Length)..];

        return userIdentityId;
    }
}