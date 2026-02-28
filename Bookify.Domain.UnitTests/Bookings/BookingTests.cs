using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.UnitTests.Users;
using Bookify.Domain.Users;
using Shouldly;

namespace Bookify.Domain.UnitTests.Bookings;

public sealed class BookingTests : BaseTest
{
    [Fact]
    public void Reserve_Should_RaiseBookingReservedDomainEvent()
    {
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        Money price = new(10.0m, Currency.Usd);
        var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
        Apartment apartment = ApartmentData.Create(price);

        var pricingService = new PricingService();

        Booking booking = Booking.Reserve(apartment, user.Id, period, DateTime.UtcNow, pricingService);

        var domainEvent = AssertDomainEventWasPublished<BookingReservedDomainEvent>(booking);

        domainEvent.BookingId.ShouldBe(booking.Id);
    }
}