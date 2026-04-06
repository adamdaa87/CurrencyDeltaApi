using CurrencyDeltaApi.Clients;
using CurrencyDeltaApi.Helpers;
using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Strategy;

/// <summary>
/// Used when either the baseline or the target currency is SEK.
///
/// • Baseline is SEK → fetch observations for the target series directly.
/// • Target is SEK   → fetch observations for the baseline series and invert each value (1 / value).
/// </summary>
public sealed class ObservationRateStrategy : IRateRetrievalStrategy
{
    private readonly IRiksbankApiClient _client;
    private readonly bool _invertValues;

    /// <param name="client">Riksbank API client.</param>
    /// <param name="invertValues">
    /// True when the target currency is SEK (values must be inverted).
    /// </param>
    public ObservationRateStrategy(IRiksbankApiClient client, bool invertValues)
    {
        _client = client;
        _invertValues = invertValues;
    }

    public async Task<List<RiksbankObservation>> GetRatesAsync(
        string baseline, string currency, DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        // When inverting, the relevant series is the baseline (e.g. sekgbppmi).
        // Otherwise the relevant series is the target currency (e.g. sekusdpmi).
        string series = _invertValues
            ? SeriesHelper.BuildSeries(baseline)
            : SeriesHelper.BuildSeries(currency);

        var observations = await _client.GetObservationsAsync(series, from, to, ct);

        if (!_invertValues)
            return observations;

        // Invert: 1 / observedValue  (SEK is the target currency)
        return observations
            .Select(o => o with { Value = Math.Round(1m / o.Value, 10) })
            .ToList();
    }
}
