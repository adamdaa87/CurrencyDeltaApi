namespace CurrencyDeltaApi.Models;

// Validerad request med starka typer
public sealed record ValidatedCurrencyDeltaRequest(
    string Baseline,
    List<string> Currencies,
    DateOnly FromDate,
    DateOnly ToDate);
