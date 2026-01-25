using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews;

internal static class ReviewErrors
{
    internal static readonly Error NotEligible = new(
        "Review.NotEligible",
        "The review is not eligible because the booking is not yet completed");
}