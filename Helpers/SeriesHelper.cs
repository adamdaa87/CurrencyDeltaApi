namespace CurrencyDeltaApi.Helpers;

// Bygger Riksbankens serie-identifierare: sek + valuta + pmi
public static class SeriesHelper
{
    public static string BuildSeries(string currencyCode) =>
        $"sek{currencyCode.ToLowerInvariant()}pmi";
}
