using System.Net.Http.Headers;
using System.Net.Http.Json;
using Bookify.Api.FunctionalTests.Infrastructure;
using Bookify.Application.Users.GetCurrentUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shouldly;

namespace Bookify.Api.FunctionalTests.Users;

public sealed class GetLoggedinUserTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Get_Should_ReturnUser_When_AccessTokenIsNotMissing()
    {
        string accessToken = await GetAccessToken();

        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);

        var user = await HttpClient.GetFromJsonAsync<UserDto>("api/users/me");

        user.ShouldNotBeNull();
    }
}