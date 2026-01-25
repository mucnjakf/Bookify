namespace Bookify.Domain.Apartments;

internal sealed record Address(
    string Country,
    string State,
    string ZipCode,
    string City,
    string Street);