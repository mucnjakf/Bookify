using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

internal sealed record BookingRejectedDomainEvent(Guid BookingId) : IDomainEvent;