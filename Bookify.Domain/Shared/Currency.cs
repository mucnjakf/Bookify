namespace Bookify.Domain.Shared;

public sealed record Currency
{
    internal static readonly Currency None = new(string.Empty);
    private static readonly Currency Usd = new("USD");
    private static readonly Currency Eur = new("EUR");

    private static readonly IReadOnlyCollection<Currency> All = [Usd, Eur];

    public string Code { get; init; }

    private Currency(string code) => Code = code;

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(currency => currency.Code == code) ??
               throw new ApplicationException("The currency code is invalid");
    }
}