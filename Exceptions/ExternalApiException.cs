namespace CurrencyDeltaApi.Exceptions;

/// <summary>
/// Thrown when the external Riksbank API call fails or returns unexpected data.
/// </summary>
public sealed class ExternalApiException : Exception
{
    public ExternalApiException(string message) : base(message) { }
    public ExternalApiException(string message, Exception inner) : base(message, inner) { }
}
