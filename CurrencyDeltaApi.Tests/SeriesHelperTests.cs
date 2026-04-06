using CurrencyDeltaApi.Helpers;

namespace CurrencyDeltaApi.Tests;

public class SeriesHelperTests
{
    [Theory]
    [InlineData("GBP", "sekgbppmi")]
    [InlineData("USD", "sekusdpmi")]
    [InlineData("EUR", "sekeurpmi")]
    public void BuildSeries_ReturnsExpectedFormat(string currencyCode, string expected)
    {
        string result = SeriesHelper.BuildSeries(currencyCode);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BuildSeries_UpperCaseInput_ReturnsLowerCase()
    {
        string result = SeriesHelper.BuildSeries("JPY");

        Assert.Equal("sekjpypmi", result);
    }
}
