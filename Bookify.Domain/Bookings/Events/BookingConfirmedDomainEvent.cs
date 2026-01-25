using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

internal sealed record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent;