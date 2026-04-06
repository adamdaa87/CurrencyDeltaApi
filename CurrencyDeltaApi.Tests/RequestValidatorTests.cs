using CurrencyDeltaApi.Exceptions;
using CurrencyDeltaApi.Models;
using CurrencyDeltaApi.Validation;

namespace CurrencyDeltaApi.Tests;

public class RequestValidatorTests
{
    private readonly RequestValidator _validator = new();

    private static CurrencyDeltaRequest ValidRequest() => new(
        Baseline: "SEK",
        Currencies: ["USD", "EUR"],
        FromDate: "2024-01-02",
        ToDate: "2024-06-01");

    [Fact]
    public void Validate_ValidRequest_ReturnsValidatedRequest()
    {
        var result = _validator.Validate(ValidRequest());

        Assert.Equal("SEK", result.Baseline);
        Assert.Equal(["USD", "EUR"], result.Currencies);
        Assert.Equal(new DateOnly(2024, 1, 2), result.FromDate);
        Assert.Equal(new DateOnly(2024, 6, 1), result.ToDate);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_MissingBaseline_Throws(string? baseline)
    {
        var request = ValidRequest() with { Baseline = baseline };

        var ex = Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
        Assert.Contains("Baseline", ex.Message);
    }

    [Fact]
    public void Validate_UnsupportedBaseline_Throws()
    {
        var request = ValidRequest() with { Baseline = "XYZ" };

        var ex = Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
        Assert.Contains("Unsupported baseline", ex.Message);
    }

    [Fact]
    public void Validate_NullCurrencies_Throws()
    {
        var request = ValidRequest() with { Currencies = null };

        Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
    }

    [Fact]
    public void Validate_EmptyCurrencies_Throws()
    {
        var request = ValidRequest() with { Currencies = [] };

        Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
    }

    [Fact]
    public void Validate_UnsupportedTargetCurrency_Throws()
    {
        var request = ValidRequest() with { Currencies = ["XYZ"] };

        var ex = Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
        Assert.Contains("Unsupported currency", ex.Message);
    }

    [Fact]
    public void Validate_DuplicateCurrency_Throws()
    {
        var request = ValidRequest() with { Currencies = ["USD", "USD"] };

        var ex = Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
        Assert.Contains("Duplicate", ex.Message);
    }

    [Fact]
    public void Validate_BaselineInCurrencies_Throws()
    {
        var request = ValidRequest() with { Baseline = "SEK", Currencies = ["SEK"] };

        var ex = Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
        Assert.Contains("must not appear", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_MissingFromDate_Throws(string? fromDate)
    {
        var request = ValidRequest() with { FromDate = fromDate };

        Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_MissingToDate_Throws(string? toDate)
    {
        var request = ValidRequest() with { ToDate = toDate };

        Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
    }

    [Fact]
    public void Validate_InvalidFromDateFormat_Throws()
    {
        var request = ValidRequest() with { FromDate = "01/02/2024" };

        var ex = Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
        Assert.Contains("yyyy-MM-dd", ex.Message);
    }

    [Fact]
    public void Validate_FromDateBeforeEarliest_Throws()
    {
        var request = ValidRequest() with { FromDate = "2022-12-31" };

        var ex = Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
        Assert.Contains("2023-01-01", ex.Message);
    }

    [Fact]
    public void Validate_ToDateNotAfterFromDate_Throws()
    {
        var request = ValidRequest() with { FromDate = "2024-06-01", ToDate = "2024-01-01" };

        var ex = Assert.Throws<CurrencyValidationException>(() => _validator.Validate(request));
        Assert.Contains("greater than", ex.Message);
    }

    [Fact]
    public void Validate_NormalizesCurrenciesToUpperCase()
    {
        var request = ValidRequest() with { Baseline = "sek", Currencies = ["usd"] };

        var result = _validator.Validate(request);

        Assert.Equal("SEK", result.Baseline);
        Assert.Equal("USD", result.Currencies[0]);
    }
}
