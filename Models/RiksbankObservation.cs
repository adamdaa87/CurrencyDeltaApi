using System.Text.Json.Serialization;

namespace CurrencyDeltaApi.Models;

/// <summary>
/// A single observation returned by the Riksbank SWEA v1 API.
/// Handles both string and numeric JSON representations of the value field.
/// </summary>
public sealed record RiksbankObservation
{
    [JsonPropertyName("date")]
    public DateOnly Date { get; init; }

    [JsonPropertyName("value")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal Value { get; init; }
}
