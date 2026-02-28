using Bookify.Domain.Users;

namespace Bookify.Application.UnitTests.Users;

internal sealed class UserData
{
    internal static readonly FirstName FirstName = new("John");
    internal static readonly LastName LastName = new("Doe");
    internal static readonly Email Email = new("test@test.com");

    internal static User Create() => User.Create(FirstName, LastName, Email);
}