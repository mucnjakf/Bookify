namespace Bookify.Domain.Abstractions;

public abstract class Entity(Guid id)
{
    public Guid Id { get; init; } = id;

    private readonly List<IDomainEvent> _domainEvents = [];

    internal IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    internal void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}