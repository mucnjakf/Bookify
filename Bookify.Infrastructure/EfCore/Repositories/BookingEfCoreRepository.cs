using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Infrastructure.EfCore.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.EfCore.Repositories;

internal sealed class BookingEfCoreRepository(ApplicationDbContext dbContext)
    : EfCoreRepository<Booking>(dbContext), IBookingRepository
{
    private static readonly List<BookingStatus> ActiveBookingStatuses =
    [
        BookingStatus.Reserved,
        BookingStatus.Confirmed,
        BookingStatus.Completed
    ];

    public Task<bool> IsOverlappingAsync(
        Apartment apartment,
        DateRange duration,
        CancellationToken cancellationToken = default)
    {
        return DbContext
            .Set<Booking>()
            .AnyAsync(booking =>
                    booking.ApartmentId == apartment.Id &&
                    booking.Duration.Start <= duration.End &&
                    booking.Duration.End >= duration.Start &&
                    ActiveBookingStatuses.Contains(booking.Status),
                cancellationToken);
    }
}