using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

internal sealed record BookingCancelledDomainEvent(Guid BookingId) : IDomainEvent;