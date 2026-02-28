using System.Net;
using System.Net.Http.Json;
using Bookify.Api.Controllers.Requests;
using Bookify.Api.FunctionalTests.Infrastructure;
using Shouldly;

namespace Bookify.Api.FunctionalTests.Auth;

public sealed class RegisterUserTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Register_Should_ReturnOk_When_RequestIsValid()
    {
        var request = new RegisterUserRequest(
            "create@test.com",
            "First",
            "Last",
            "21345");

        HttpResponseMessage httpResponseMessage = await HttpClient
            .PostAsJsonAsync("api/auth/register", request);

        httpResponseMessage.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("", "first", "last", "12345")]
    [InlineData("test.com", "first", "last", "12345")]
    [InlineData("@test.com", "first", "last", "12345")]
    [InlineData("test@", "first", "last", "12345")]
    [InlineData("test@test.com", "", "last", "12345")]
    [InlineData("test@test.com", "first", "", "12345")]
    [InlineData("test@test.com", "first", "last", "")]
    public async Task Register_Should_ReturnBadRequest_When_RequestIsInvalid(
        string email,
        string firstName,
        string lastName,
        string password)
    {
        var request = new RegisterUserRequest(email, firstName, lastName, password);

        HttpResponseMessage httpResponseMessage = await HttpClient
            .PostAsJsonAsync("api/auth/register", request);

        httpResponseMessage.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}