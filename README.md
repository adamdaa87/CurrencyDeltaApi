# CurrencyDeltaApi

A .NET 10 Web API that computes exchange-rate deltas between two dates for a set of currencies, powered by the [Swedish Riksbank SWEA v1 API](https://developer.riksbank.se/).

## What it does

Given a baseline currency, one or more target currencies, and a date range, the API returns how much each exchange rate changed (delta) between those two dates.

## Endpoint

```
POST /CurrencyDelta
```

### Request

```json
{
  "baseline": "SEK",
  "currencies": ["USD", "EUR"],
  "fromDate": "2024-01-02",
  "toDate": "2024-06-01"
}
```

### Response

```json
[
  { "currency": "USD", "delta": 0.123 },
  { "currency": "EUR", "delta": -0.045 }
]
```

### Error Response

```json
{
  "errorCode": "currencyproblem",
  "errorDetails": "Unsupported currency: XYZ"
}
```

## Supported Currencies

AUD, BRL, CAD, CHF, CNY, CZK, DKK, EUR, GBP, HKD, HUF, INR, ISK, JPY, KRW, MAD, MXN, NOK, NZD, PLN, SAR, SEK, SGD, THB, TRY, USD, ZAR

## Architecture

```
Request → Controller → Validator → Service → Strategy → Riksbank API
                                                ↓
                                        Compute delta
                                                ↓
                                           Response
```

| Layer | Responsibility |
|---|---|
| **Controller** | Receives the POST request |
| **RequestValidator** | Validates currencies, date formats, and date ranges |
| **CurrencyDeltaService** | Orchestrates rate retrieval and delta calculation |
| **RateStrategyFactory** | Selects the correct strategy (direct rate vs. cross rate) |
| **ObservationRateStrategy** | Used when SEK is the baseline or target currency |
| **CrossRateStrategy** | Used when neither currency is SEK |
| **RiksbankApiClient** | HTTP client for the Riksbank SWEA v1 API |
| **ExceptionHandlingMiddleware** | Converts exceptions into consistent JSON error responses |

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run the API

```bash
dotnet run
```

The API starts at `http://localhost:5018`.

### Run the Tests

```bash
dotnet test
```

### Test with Postman or curl

```bash
curl -X POST http://localhost:5018/CurrencyDelta \
  -H "Content-Type: application/json" \
  -d '{"baseline":"SEK","currencies":["USD","EUR"],"fromDate":"2024-01-02","toDate":"2024-06-01"}'
```

## Tech Stack

- **.NET 10** — ASP.NET Core Web API
- **HttpClientFactory** — Typed HTTP client for external API calls
- **Strategy Pattern** — Selects rate retrieval logic based on currency pair
- **xUnit** — Unit testing framework
