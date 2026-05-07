using CurrencyDeltaApi.Clients;
using CurrencyDeltaApi.Helpers;
using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Strategy;

// Används när baseline eller target är SEK (hämtar direkt eller inverterar)
public sealed class ObservationRateStrategy : IRateRetrievalStrategy
{
    private readonly IRiksbankApiClient _client;
    private readonly bool _invertValues;

    // invertValues = true när target är SEK (värden ska inverteras)
    public ObservationRateStrategy(IRiksbankApiClient client, bool invertValues)
    {
        _client = client;
        _invertValues = invertValues;
    }

    public async Task<List<RiksbankObservation>> GetRatesAsync(
        string baseline, string currency, DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        // Vid invertering används baseline-serien, annars target-serien
        string series = _invertValues
            ? SeriesHelper.BuildSeries(baseline)
            : SeriesHelper.BuildSeries(currency);

        var observations = await _client.GetObservationsAsync(series, from, to, ct);

        if (!_invertValues)
            return observations;

        // Invertera: 1 / värde (när SEK är target)
        return observations
            .Select(o => o with { Value = Math.Round(1m / o.Value, 10) })
            .ToList();
    }
}
