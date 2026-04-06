using CurrencyDeltaApi.Helpers;
using CurrencyDeltaApi.Models;
using CurrencyDeltaApi.Strategy;

namespace CurrencyDeltaApi.Services;

public sealed class CurrencyDeltaService : ICurrencyDeltaService
{
    private readonly IRateStrategyFactory _strategyFactory;

    public CurrencyDeltaService(IRateStrategyFactory strategyFactory)
    {
        _strategyFactory = strategyFactory;
    }

    public async Task<List<CurrencyDeltaResponse>> GetDeltasAsync(
        ValidatedCurrencyDeltaRequest request, CancellationToken ct = default)
    {
        var results = new List<CurrencyDeltaResponse>(request.Currencies.Count);

        foreach (string currency in request.Currencies)
        {
            IRateRetrievalStrategy strategy = _strategyFactory.Create(request.Baseline, currency);

            var observations = await strategy.GetRatesAsync(
                request.Baseline, currency, request.FromDate, request.ToDate, ct);

            // When the requested dates fall on non-bank days, use the nearest available date.
            var fromObservation = NearestDateHelper.FindNearest(observations, request.FromDate);
            var toObservation = NearestDateHelper.FindNearest(observations, request.ToDate);

            // Delta formula: rateAtToDate − rateAtFromDate
            decimal delta = Math.Round(toObservation.Value - fromObservation.Value, 3);

            results.Add(new CurrencyDeltaResponse(currency, delta));
        }

        return results;
    }
}
