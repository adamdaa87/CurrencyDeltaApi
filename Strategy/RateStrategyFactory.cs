using CurrencyDeltaApi.Clients;

namespace CurrencyDeltaApi.Strategy;

/// <summary>
/// Factory that selects the appropriate <see cref="IRateRetrievalStrategy"/>
/// based on whether SEK is involved as the baseline or target currency.
/// </summary>
public interface IRateStrategyFactory
{
    IRateRetrievalStrategy Create(string baseline, string currency);
}

public sealed class RateStrategyFactory : IRateStrategyFactory
{
    private readonly IRiksbankApiClient _client;

    public RateStrategyFactory(IRiksbankApiClient client)
    {
        _client = client;
    }

    public IRateRetrievalStrategy Create(string baseline, string currency)
    {
        bool isSekBaseline = baseline.Equals("SEK", StringComparison.OrdinalIgnoreCase);
        bool isSekTarget = currency.Equals("SEK", StringComparison.OrdinalIgnoreCase);

        if (isSekBaseline || isSekTarget)
        {
            // invertValues = true only when the requested currency is SEK
            return new ObservationRateStrategy(_client, invertValues: isSekTarget);
        }

        return new CrossRateStrategy(_client);
    }
}
