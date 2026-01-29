using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings;

public static class BookingErrors
{
    public static Error NotFound = new(
        "Booking.Found",
        "The booking with the specified ID was not found");

    public static Error Overlap = new(
        "Booking.Overlap",
        "The current booking is overlapping with an existing one");

    internal static Error NotReserved = new(
        "Booking.NotReserved",
        "The booking is not reserved");

    internal static Error NotConfirmed = new(
        "Booking.NotConfirmed",
        "The booking is not confirmed");

    internal static Error AlreadyStarted = new(
        "Booking.AlreadyStarted",
        "The booking has already started");
}