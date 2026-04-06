using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Services;

/// <summary>
/// Core service that computes exchange-rate deltas between two dates
/// for a list of currencies relative to a baseline currency.
/// </summary>
public interface ICurrencyDeltaService
{
    Task<List<CurrencyDeltaResponse>> GetDeltasAsync(
        ValidatedCurrencyDeltaRequest request, CancellationToken ct = default);
}
