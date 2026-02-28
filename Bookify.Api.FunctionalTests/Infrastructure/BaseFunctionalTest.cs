using System.Net.Http.Json;
using Bookify.Api.Controllers.Requests;
using Bookify.Api.FunctionalTests.Users;
using Bookify.Application.Auth.LoginUser;

namespace Bookify.Api.FunctionalTests.Infrastructure;

public abstract class BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    : IClassFixture<FunctionalTestWebAppFactory>
{
    protected readonly HttpClient HttpClient = factory.CreateClient();

    protected async Task<string> GetAccessToken()
    {
        HttpResponseMessage httpResponseMessage = await HttpClient.PostAsJsonAsync("api/auth/login",
            new LoginUserRequest(
                UserData.RegisterTestUserRequest.Email,
                UserData.RegisterTestUserRequest.Password));

        var token = await httpResponseMessage.Content.ReadFromJsonAsync<TokenDto>();

        return token!.AccessToken;
    }
}