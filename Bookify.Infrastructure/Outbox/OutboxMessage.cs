namespace Bookify.Infrastructure.Outbox;

internal sealed class OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content)
{
    public Guid Id { get; private set; } = id;

    public DateTime OccurredOnUtc { get; private set; } = occurredOnUtc;

    public string Type { get; private set; } = type;

    public string Content { get; private set; } = content;

    public DateTime? ProcessedOnUtc { get; private set; }

    public string? Error { get; private set; }
}