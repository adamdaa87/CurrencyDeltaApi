namespace CurrencyDeltaApi.Models;

// Response med valutakod och beräknat delta
public sealed record CurrencyDeltaResponse(string Currency, decimal Delta);
