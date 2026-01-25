using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

internal sealed record BookingCompletedDomainEvent(Guid BookingId) : IDomainEvent;