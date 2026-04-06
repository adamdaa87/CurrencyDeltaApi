namespace CurrencyDeltaApi.Models;

/// <summary>
/// Standardized error envelope returned on validation or processing failures.
/// </summary>
public sealed record ErrorResponse(string ErrorCode, string ErrorDetails);
