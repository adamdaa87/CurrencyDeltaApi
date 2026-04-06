using System.Net.Http.Json;
using System.Text.Json;
using CurrencyDeltaApi.Exceptions;
using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Clients;

public sealed class RiksbankApiClient : IRiksbankApiClient
{
    private readonly HttpClient _http;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
    };

    public RiksbankApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<RiksbankObservation>> GetObservationsAsync(
        string series, DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        string url = $"observations/{series}/{Format(from)}/{Format(to)}";
        return await FetchAsync(url, ct);
    }

    public async Task<List<RiksbankObservation>> GetCrossRatesAsync(
        string baselineSeries, string targetSeries, DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        string url = $"CrossRates/{baselineSeries}/{targetSeries}/{Format(from)}/{Format(to)}";
        return await FetchAsync(url, ct);
    }

    private async Task<List<RiksbankObservation>> FetchAsync(string relativeUrl, CancellationToken ct)
    {
        HttpResponseMessage response;
        try
        {
            response = await _http.GetAsync(relativeUrl, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new ExternalApiException($"Riksbank API request failed: {ex.Message}", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            string body = await response.Content.ReadAsStringAsync(ct);
            throw new ExternalApiException(
                $"Riksbank API returned {(int)response.StatusCode}: {body}");
        }

        var observations = await response.Content.ReadFromJsonAsync<List<RiksbankObservation>>(JsonOptions, ct);

        if (observations is null || observations.Count == 0)
            throw new ExternalApiException(
                $"Riksbank API returned no observations for {relativeUrl}");

        return observations;
    }

    private static string Format(DateOnly d) => d.ToString("yyyy-MM-dd");
}
