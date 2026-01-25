using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

internal sealed class Booking : Entity
{
    internal Guid ApartmentId { get; private set; }

    internal Guid UserId { get; private set; }

    internal DateRange Duration { get; private set; }

    internal Money PriceForPeriod { get; private set; }

    internal Money CleaningFee { get; private set; }

    internal Money AmenitiesUpCharge { get; private set; }

    internal Money TotalPrice { get; private set; }

    internal BookingStatus Status { get; private set; }

    internal DateTime CreatedOnUtc { get; private set; }

    internal DateTime? ConfirmedOnUtc { get; private set; }

    internal DateTime? RejectedOnUtc { get; private set; }

    internal DateTime? CompletedOnUtc { get; private set; }

    internal DateTime? CancelledOnUtc { get; private set; }

    private Booking(
        Guid id,
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        Money priceForPeriod,
        Money cleaningFee,
        Money amenitiesUpCharge,
        Money totalPrice,
        DateTime createdOnUtc) : base(id)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        CreatedOnUtc = createdOnUtc;
    }

    internal static Booking Reserve(
        Apartment apartment,
        Guid userId,
        DateRange duration,
        DateTime utcNow,
        PricingService pricingService)
    {
        PricingDetails pricingDetails = PricingService.CalculatePrice(apartment, duration);

        var booking = new Booking(
            Guid.NewGuid(),
            apartment.Id,
            userId,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpCharge,
            pricingDetails.TotalPrice,
            utcNow);

        booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));

        apartment.LastBookedOnUtc = utcNow;

        return booking;
    }

    internal Result Confirm(DateTime utcNow)
    {
        if (Status is not BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = utcNow;

        RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));

        return Result.Success();
    }

    internal Result Reject(DateTime utcNow)
    {
        if (Status is not BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }

        Status = BookingStatus.Rejected;
        RejectedOnUtc = utcNow;

        RaiseDomainEvent(new BookingRejectedDomainEvent(Id));

        return Result.Success();
    }

    internal Result Complete(DateTime utcNow)
    {
        if (Status is not BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }

        Status = BookingStatus.Completed;
        ConfirmedOnUtc = utcNow;

        RaiseDomainEvent(new BookingCompletedDomainEvent(Id));

        return Result.Success();
    }

    internal Result Cancel(DateTime utcNow)
    {
        if (Status is not BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }

        DateOnly currentDate = DateOnly.FromDateTime(utcNow);

        if (currentDate > Duration.Start)
        {
            return Result.Failure(BookingErrors.AlreadyStarted);
        }

        Status = BookingStatus.Cancelled;
        ConfirmedOnUtc = utcNow;

        RaiseDomainEvent(new BookingCancelledDomainEvent(Id));

        return Result.Success();
    }
}