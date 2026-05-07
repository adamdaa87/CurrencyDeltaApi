namespace CurrencyDeltaApi.Exceptions;

// Kastas vid valideringsfel med felkod och felmeddelande
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
