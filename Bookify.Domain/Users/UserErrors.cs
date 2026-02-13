using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users;

public static class UserErrors
{
    public static Error NotFound = new(
        "User.Found",
        "The user with the specified ID was not found");

    public static Error InvalidCredentials = new("User.Credentials", "The provided credentials are invalid");
}