namespace CurrencyDeltaApi.Exceptions;

// Kastas vid problem med Riksbankens API
public sealed class ExternalApiException : Exception
{
    public ExternalApiException(string message) : base(message) { }
    public ExternalApiException(string message, Exception inner) : base(message, inner) { }
}
