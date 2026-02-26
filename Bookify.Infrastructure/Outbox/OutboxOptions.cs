namespace Bookify.Infrastructure.Outbox;

internal sealed class OutboxOptions
{
    internal const string SectionName = "Outbox";

    public required int IntervalInSeconds { get; init; }

    public required int BatchSize { get; init; }
}