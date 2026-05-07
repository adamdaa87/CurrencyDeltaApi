using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Strategy;

// Strategi för att hämta valutakurser (direkt eller via korskurs)
public interface IRateRetrievalStrategy
{
    Task<List<RiksbankObservation>> GetRatesAsync(
        string baseline, string currency, DateOnly from, DateOnly to, CancellationToken ct = default);
}
