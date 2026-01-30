namespace Bookify.Domain.Abstractions;

public abstract class Entity
{
    public Guid Id { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity() { }

    protected Entity(Guid id) => Id = id;

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}