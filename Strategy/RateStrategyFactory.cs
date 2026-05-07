using CurrencyDeltaApi.Clients;

namespace CurrencyDeltaApi.Strategy;

// Skapar rätt strategi baserat på om SEK är baseline eller target
public interface IRateStrategyFactory
{
    IRateRetrievalStrategy Create(string baseline, string currency);
}

// Factory-implementation som väljer ObservationRateStrategy eller CrossRateStrategy
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

        // Om SEK är baseline eller target, använd direkt hämtning
        if (isSekBaseline || isSekTarget)
        {
            // Invertera värden endast när SEK är target
            return new ObservationRateStrategy(_client, invertValues: isSekTarget);
        }

        // Annars använd korskurs via SEK
        return new CrossRateStrategy(_client);
    }
}
