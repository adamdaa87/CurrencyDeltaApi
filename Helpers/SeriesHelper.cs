namespace CurrencyDeltaApi.Helpers;

/// <summary>
/// Utility for constructing Riksbank series identifiers.
/// Series naming rule: "sek" + lowercase(currency) + "pmi"
/// Example: GBP → sekgbppmi
/// </summary>
public static class SeriesHelper
{
    public static string BuildSeries(string currencyCode) =>
        $"sek{currencyCode.ToLowerInvariant()}pmi";
}
