namespace Bookify.Application.Apartments.SearchApartments;

internal sealed record ApartmentResponse
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public decimal Price { get; init; }

    public required string Currency { get; init; }

    public required AddressResponse Address { get; set; }
}