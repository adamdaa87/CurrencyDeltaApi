using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Clients;

/// <summary>
/// Abstraction over the Riksbank SWEA v1 REST API.
/// </summary>
public interface IRiksbankApiClient
{
    /// <summary>
    /// GET /swea/v1/observations/{series}/{from}/{to}
    /// </summary>
    Task<List<RiksbankObservation>> GetObservationsAsync(
        string series, DateOnly from, DateOnly to, CancellationToken ct = default);

    /// <summary>
    /// GET /swea/v1/CrossRates/{baselineSeries}/{targetSeries}/{from}/{to}
    /// </summary>
    Task<List<RiksbankObservation>> GetCrossRatesAsync(
        string baselineSeries, string targetSeries, DateOnly from, DateOnly to, CancellationToken ct = default);
}
