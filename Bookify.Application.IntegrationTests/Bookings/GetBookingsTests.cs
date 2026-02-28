using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.IntegrationTests.Infrastructure;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Shouldly;

namespace Bookify.Application.IntegrationTests.Bookings;

public sealed class GetBookingsTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private static readonly Guid BookingId = Guid.NewGuid();

    [Fact]
    public async Task GetBooking_Should_ReturnFailure_When_BookingIsNotFound()
    {
        var query = new GetBookingQuery(BookingId);

        Result<BookingDto> result = await Sender.Send(query);

        result.Error.ShouldBe(BookingErrors.NotFound);
    }
}