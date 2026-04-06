namespace CurrencyDeltaApi.Models;

/// <summary>
/// Strongly-typed request produced after successful validation.
/// </summary>
public sealed record ValidatedCurrencyDeltaRequest(
    string Baseline,
    List<string> Currencies,
    DateOnly FromDate,
    DateOnly ToDate);
