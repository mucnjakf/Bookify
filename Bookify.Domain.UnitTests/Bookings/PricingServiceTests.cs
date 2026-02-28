using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartments;
using Shouldly;

namespace Bookify.Domain.UnitTests.Bookings;

public sealed class PricingServiceTests
{
    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice()
    {
        Money price = new(10.0m, Currency.Usd);
        var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
        Money expectedTotalPrice = price with { Amount = price.Amount * period.LengthInDays };
        Apartment apartment = ApartmentData.Create(price);

        var pricingService = new PricingService();

        PricingDetails pricingDetails = pricingService.CalculatePrice(apartment, period);

        pricingDetails.TotalPrice.ShouldBe(expectedTotalPrice);
    }

    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice_When_CleaningFeeIsIncluded()
    {
        Money price = new(10.0m, Currency.Usd);
        Money cleaningFee = new(99.99m, Currency.Usd);
        var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
        Money expectedTotalPrice = price with { Amount = price.Amount * period.LengthInDays + cleaningFee.Amount };
        Apartment apartment = ApartmentData.Create(price, cleaningFee);

        var pricingService = new PricingService();

        PricingDetails pricingDetails = pricingService.CalculatePrice(apartment, period);

        pricingDetails.TotalPrice.ShouldBe(expectedTotalPrice);
    }
}