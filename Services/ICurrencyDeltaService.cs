using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Services;

// Tjänst som beräknar valutakursdeltan mellan två datum
public interface ICurrencyDeltaService
{
    Task<List<CurrencyDeltaResponse>> GetDeltasAsync(
        ValidatedCurrencyDeltaRequest request, CancellationToken ct = default);
}
