using Bookify.Domain.Users;

namespace Bookify.Domain.UnitTests.Users;

internal static class UserData
{
    internal static readonly FirstName FirstName = new("John");
    internal static readonly LastName LastName = new("Doe");
    internal static readonly Email Email = new("test@test.com");
}