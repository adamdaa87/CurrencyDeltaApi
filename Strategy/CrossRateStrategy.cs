using CurrencyDeltaApi.Clients;
using CurrencyDeltaApi.Helpers;
using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Strategy;

/// <summary>
/// Used when neither the baseline nor the target currency is SEK.
/// Delegates to the Riksbank CrossRates endpoint which computes the cross rate
/// from the two underlying SEK-denominated series.
/// </summary>
public sealed class CrossRateStrategy : IRateRetrievalStrategy
{
    private readonly IRiksbankApiClient _client;

    public CrossRateStrategy(IRiksbankApiClient client)
    {
        _client = client;
    }

    public async Task<List<RiksbankObservation>> GetRatesAsync(
        string baseline, string currency, DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        string baselineSeries = SeriesHelper.BuildSeries(baseline);
        string targetSeries = SeriesHelper.BuildSeries(currency);

        return await _client.GetCrossRatesAsync(baselineSeries, targetSeries, from, to, ct);
    }
}
