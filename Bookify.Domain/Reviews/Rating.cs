using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews;

internal sealed record Rating
{
    internal int Value { get; init; }

    private Rating(int value) => Value = value;

    internal static Result<Rating> Create(int value) => value is < 1 or > 5
        ? Result.Failure<Rating>(RatingErrors.Invalid)
        : new Rating(value);
}