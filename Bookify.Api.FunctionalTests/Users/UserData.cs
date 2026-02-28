using Bookify.Api.Controllers.Requests;

namespace Bookify.Api.FunctionalTests.Users;

internal static class UserData
{
    public static readonly RegisterUserRequest RegisterTestUserRequest = new(
        "test@test.com",
        "John",
        "Doe",
        "pass123!");
}