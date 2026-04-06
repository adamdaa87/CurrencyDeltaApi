namespace CurrencyDeltaApi.Exceptions;

/// <summary>
/// Thrown when request validation fails.
/// Carries a machine-readable error code and a human-readable detail message.
/// </summary>
public sealed class CurrencyValidationException : Exception
{
    public string ErrorCode { get; }
    public string ErrorDetails { get; }

    public CurrencyValidationException(string errorCode, string errorDetails)
        : base(errorDetails)
    {
        ErrorCode = errorCode;
        ErrorDetails = errorDetails;
    }
}
