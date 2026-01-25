namespace Bookify.Domain.Bookings;

internal sealed record DateRange
{
    internal DateOnly Start { get; init; }

    internal DateOnly End { get; init; }

    internal int LengthInDays => End.DayNumber - Start.DayNumber;

    private DateRange() { }

    internal static DateRange Create(DateOnly start, DateOnly end)
    {
        if (start > end)
        {
            throw new ApplicationException("End date precedes start date");
        }

        return new DateRange
        {
            Start = start,
            End = end
        };
    }
}