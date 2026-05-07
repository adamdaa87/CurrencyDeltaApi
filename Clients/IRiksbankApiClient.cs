using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Clients;

// HTTP-klient för Riksbankens SWEA v1 API
public interface IRiksbankApiClient
{
    // Hämtar observationer för en serie
    Task<List<RiksbankObservation>> GetObservationsAsync(string series, DateOnly from, DateOnly to, CancellationToken ct = default);

    // Hämtar korskurser mellan två serier
    Task<List<RiksbankObservation>> GetCrossRatesAsync(
        string baselineSeries, string targetSeries, DateOnly from, DateOnly to, CancellationToken ct = default);
}
