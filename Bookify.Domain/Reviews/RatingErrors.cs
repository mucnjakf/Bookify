using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews;

internal static class RatingErrors
{
    internal static readonly Error Invalid = new("Rating.Invalid", "The rating is invalid");
}