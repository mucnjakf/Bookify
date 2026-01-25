using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

internal sealed record PricingDetails(
    Money PriceForPeriod,
    Money CleaningFee,
    Money AmenitiesUpCharge,
    Money TotalPrice);