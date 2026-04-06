using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Strategy;

/// <summary>
/// Strategy interface for retrieving exchange-rate observations.
/// Different implementations handle SEK-baseline, SEK-target, and cross-rate scenarios.
/// </summary>
public interface IRateRetrievalStrategy
{
    Task<List<RiksbankObservation>> GetRatesAsync(
        string baseline, string currency, DateOnly from, DateOnly to, CancellationToken ct = default);
}
