namespace CurrencyDeltaApi.Models;

// Standardiserat felsvar vid valideringsfel eller API-problem
public sealed record ErrorResponse(string ErrorCode, string ErrorDetails);
