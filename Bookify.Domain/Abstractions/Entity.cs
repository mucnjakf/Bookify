namespace Bookify.Domain.Abstractions;

internal abstract class Entity(Guid id)
{
    internal Guid Id { get; init; } = id;

    private readonly List<IDomainEvent> _domainEvents = [];

    internal IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    internal void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}