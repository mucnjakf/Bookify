using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authentication;

internal sealed class AdminAuthorizationDelegatingHandler(IOptions<KeycloakOptions> keycloakOptions) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        AuthorizationToken authorizationToken = await GetAuthorizationTokenAsync(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            authorizationToken.AccessToken);

        HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return httpResponseMessage;
    }

    private async Task<AuthorizationToken> GetAuthorizationTokenAsync(CancellationToken cancellationToken)
    {
        var authorizationRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", keycloakOptions.Value.AdminClientId),
            new("client_secret", keycloakOptions.Value.AdminClientSecret),
            new("scope", "openid email"),
            new("grant_type", "client_credentials")
        };

        var authorizationContentRequest = new FormUrlEncodedContent(authorizationRequestParameters);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(keycloakOptions.Value.TokenUrl))
        {
            Content = authorizationContentRequest
        };

        HttpResponseMessage httpResponseMessage = await base.SendAsync(httpRequestMessage, cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        AuthorizationToken authorizationToken = await httpResponseMessage.Content
            .ReadFromJsonAsync<AuthorizationToken>(cancellationToken) ?? throw new ApplicationException();

        return authorizationToken;
    }
}