using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Validation;

// Validerar inkommande requests och returnerar validerad request
public interface IRequestValidator
{
    ValidatedCurrencyDeltaRequest Validate(CurrencyDeltaRequest request);
}
