namespace Bookify.Domain.Shared;

internal sealed record Currency
{
    internal static readonly Currency None = new(string.Empty);
    internal static readonly Currency Usd = new("USD");
    internal static readonly Currency Eur = new("EUR");

    internal static readonly IReadOnlyCollection<Currency> All = [Usd, Eur];

    internal string Code { get; init; }

    private Currency(string code) => Code = code;

    internal static Currency FromCode(string code)
    {
        return All.FirstOrDefault(currency => currency.Code == code) ??
               throw new ApplicationException("The currency code is invalid");
    }
}