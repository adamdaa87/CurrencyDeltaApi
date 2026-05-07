using System.Text.Json.Serialization;

namespace CurrencyDeltaApi.Models;

// En enskild observation från Riksbankens API
public sealed record RiksbankObservation
{
    [JsonPropertyName("date")]
    public DateOnly Date { get; init; }

    [JsonPropertyName("value")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal Value { get; init; }
}
