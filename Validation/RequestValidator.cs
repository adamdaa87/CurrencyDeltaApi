using System.Globalization;
using CurrencyDeltaApi.Exceptions;
using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Validation;

public sealed class RequestValidator : IRequestValidator
{
    /// <summary>
    /// Currency codes for which Riksbank publishes PMI series.
    /// </summary>
    private static readonly HashSet<string> SupportedCurrencies = new(StringComparer.OrdinalIgnoreCase)
    {
        "AUD", "BRL", "CAD", "CHF", "CNY", "CZK", "DKK", "EUR", "GBP",
        "HKD", "HUF", "INR", "ISK", "JPY", "KRW", "MAD", "MXN", "NOK",
        "NZD", "PLN", "SAR", "SEK", "SGD", "THB", "TRY", "USD", "ZAR"
    };

    private static readonly DateOnly EarliestAllowedDate = new(2023, 1, 1);

    public ValidatedCurrencyDeltaRequest Validate(CurrencyDeltaRequest request)
    {
        // --- Null / empty checks ---
        if (string.IsNullOrWhiteSpace(request.Baseline))
            throw new CurrencyValidationException("currencyproblem", "Baseline currency is required");

        if (request.Currencies is null || request.Currencies.Count == 0)
            throw new CurrencyValidationException("currencyproblem", "At least one target currency is required");

        string baseline = request.Baseline.Trim().ToUpperInvariant();

        // --- Currency validation ---
        if (!SupportedCurrencies.Contains(baseline))
            throw new CurrencyValidationException("currencyproblem", $"Unsupported baseline currency: {baseline}");

        var normalizedCurrencies = new List<string>(request.Currencies.Count);
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (string raw in request.Currencies)
        {
            string currency = raw.Trim().ToUpperInvariant();

            if (!SupportedCurrencies.Contains(currency))
                throw new CurrencyValidationException("currencyproblem", $"Unsupported currency: {currency}");

            if (!seen.Add(currency))
                throw new CurrencyValidationException("currencyproblem", $"Duplicate currency: {currency}");

            if (currency.Equals(baseline, StringComparison.OrdinalIgnoreCase))
                throw new CurrencyValidationException("currencyproblem", "Baseline currency must not appear in the currencies list");

            normalizedCurrencies.Add(currency);
        }

        // --- Date validation ---
        if (string.IsNullOrWhiteSpace(request.FromDate))
            throw new CurrencyValidationException("dateproblem", "From date is required");

        if (string.IsNullOrWhiteSpace(request.ToDate))
            throw new CurrencyValidationException("dateproblem", "To date is required");

        if (!DateOnly.TryParseExact(request.FromDate.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly fromDate))
            throw new CurrencyValidationException("dateproblem", "From date must be a valid date in yyyy-MM-dd format");

        if (!DateOnly.TryParseExact(request.ToDate.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly toDate))
            throw new CurrencyValidationException("dateproblem", "To date must be a valid date in yyyy-MM-dd format");

        if (fromDate < EarliestAllowedDate)
            throw new CurrencyValidationException("dateproblem", $"From date must not be earlier than {EarliestAllowedDate:yyyy-MM-dd}");

        if (toDate <= fromDate)
            throw new CurrencyValidationException("dateproblem", "To date must be greater than from date");

        return new ValidatedCurrencyDeltaRequest(baseline, normalizedCurrencies, fromDate, toDate);
    }
}
