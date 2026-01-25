using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

internal sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;