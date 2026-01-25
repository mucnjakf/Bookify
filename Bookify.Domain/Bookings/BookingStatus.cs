namespace Bookify.Domain.Bookings;

internal enum BookingStatus
{
    Reserved = 0,
    Confirmed = 1,
    Rejected = 2,
    Cancelled = 3,
    Completed = 4
}