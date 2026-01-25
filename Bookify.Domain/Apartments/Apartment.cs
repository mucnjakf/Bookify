using Bookify.Domain.Abstractions;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Apartments;

internal sealed class Apartment : Entity
{
    internal Name Name { get; private set; }

    internal Description Description { get; private set; }

    internal Address Address { get; private set; }

    internal Money Price { get; private set; }

    internal Money CleaningFee { get; private set; }

    public DateTime? LastBookedOnUtc { get; internal set; }

    internal List<Amenity> Amenities { get; private set; } = [];

    internal Apartment(
        Guid id,
        Name name,
        Description description,
        Address address,
        Money price,
        Money cleaningFee,
        List<Amenity> amenities)
        : base(id)
    {
        Name = name;
        Description = description;
        Address = address;
        Price = price;
        CleaningFee = cleaningFee;
        Amenities = amenities;
    }
}