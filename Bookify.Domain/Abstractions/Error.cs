namespace Bookify.Domain.Abstractions;

public sealed record Error(string Code, string Message)
{
    internal static Error None = new(string.Empty, string.Empty);

    internal static Error NullValue = new("Error.NullValue", "Null value was provided");
}