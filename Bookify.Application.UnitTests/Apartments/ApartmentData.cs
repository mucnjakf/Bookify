using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Application.UnitTests.Apartments;

internal sealed class ApartmentData
{
    internal static Apartment Create() => new(
        Guid.NewGuid(),
        new Name("Test apartment"),
        new Description("Test description"),
        new Address("Country", "State", "Zip code", "City", "Street"),
        new Money(100.0m, Currency.Usd),
        Money.Zero(),
        []);
}