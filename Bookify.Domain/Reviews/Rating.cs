using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews;

public sealed record Rating
{
    public int Value { get; init; }

    private Rating(int value) => Value = value;

    public static Result<Rating> Create(int value) => value is < 1 or > 5
        ? Result.Failure<Rating>(RatingErrors.Invalid)
        : new Rating(value);
}