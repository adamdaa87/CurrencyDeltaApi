namespace CurrencyDeltaApi.Models;

/// <summary>
/// Inbound request for computing exchange-rate deltas.
/// All properties are nullable so the validator can produce clear error messages
/// for missing fields instead of relying on model-binding defaults.
/// </summary>
public sealed record CurrencyDeltaRequest(
    string? Baseline,
    List<string>? Currencies,
    string? FromDate,
    string? ToDate);
