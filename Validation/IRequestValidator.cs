using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Validation;

/// <summary>
/// Validates inbound currency delta requests.
/// Returns a strongly-typed <see cref="ValidatedCurrencyDeltaRequest"/> on success.
/// Throws <see cref="Exceptions.CurrencyValidationException"/> on failure.
/// </summary>
public interface IRequestValidator
{
    ValidatedCurrencyDeltaRequest Validate(CurrencyDeltaRequest request);
}
