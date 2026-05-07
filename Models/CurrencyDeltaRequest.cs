namespace CurrencyDeltaApi.Models;

// Inkommande request för valutakursberäkning
public sealed record CurrencyDeltaRequest(
    string? Baseline,
    List<string>? Currencies,
    string? FromDate,
    string? ToDate);
