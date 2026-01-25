using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

internal sealed record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent;