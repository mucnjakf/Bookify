using Bookify.Domain.Apartments;

namespace Bookify.Domain.Shared;

internal sealed record Money(decimal Amount, Currency Currency)
{
    internal static Money Zero() => new(0, Currency.None);

    internal static Money Zero(Currency currency) => new(0, currency);

    internal bool IsZero() => this == Zero(Currency);

    public static Money operator +(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new InvalidOperationException("Currencies have to be equal");
        }

        return first with { Amount = first.Amount + second.Amount };
    }
}