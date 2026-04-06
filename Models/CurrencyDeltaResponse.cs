namespace CurrencyDeltaApi.Models;

/// <summary>
/// Single currency delta entry returned to the caller.
/// </summary>
public sealed record CurrencyDeltaResponse(string Currency, decimal Delta);
