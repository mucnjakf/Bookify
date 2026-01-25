namespace Bookify.Domain.Abstractions;

internal sealed record Error(string Code, string Message)
{
    internal static Error None = new(string.Empty, string.Empty);

    internal static Error NullValue = new("Error.NullValue", "Null value was provided");
}