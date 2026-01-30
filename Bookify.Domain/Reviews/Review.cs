using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews.Events;

namespace Bookify.Domain.Reviews;

public sealed class Review : Entity
{
    public Guid ApartmentId { get; private set; }

    public Guid BookingId { get; private set; }

    public Guid UserId { get; private set; }

    public Rating Rating { get; private set; }

    public string Comment { get; private set; }

    internal DateTime CreatedOnUtc { get; private set; }

    private Review(
        Guid id,
        Guid apartmentId,
        Guid bookingId,
        Guid userId,
        Rating rating,
        string comment,
        DateTime createdOnUtc) : base(id)
    {
        ApartmentId = apartmentId;
        BookingId = bookingId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedOnUtc = createdOnUtc;
    }

    internal static Result<Review> Create(
        Booking booking,
        Rating rating,
        string comment,
        DateTime createdOnUtc)
    {
        if (booking.Status is not BookingStatus.Completed)
        {
            return Result.Failure<Review>(ReviewErrors.NotEligible);
        }

        var review = new Review(
            Guid.NewGuid(),
            booking.ApartmentId,
            booking.Id,
            booking.UserId,
            rating,
            comment,
            createdOnUtc);

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

        return review;
    }
}