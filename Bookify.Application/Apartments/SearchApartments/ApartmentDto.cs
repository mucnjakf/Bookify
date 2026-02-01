namespace Bookify.Application.Apartments.SearchApartments;

public sealed record ApartmentDto
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public decimal Price { get; init; }

    public required string Currency { get; init; }

    public required AddressDto Address { get; set; }
}