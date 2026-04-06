using CurrencyDeltaApi.Helpers;
using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Tests;

public class NearestDateHelperTests
{
    [Fact]
    public void FindNearest_ExactMatch_ReturnsThatObservation()
    {
        var observations = new List<RiksbankObservation>
        {
            new() { Date = new DateOnly(2024, 1, 1), Value = 10.0m },
            new() { Date = new DateOnly(2024, 1, 2), Value = 11.0m },
            new() { Date = new DateOnly(2024, 1, 3), Value = 12.0m },
        };

        var result = NearestDateHelper.FindNearest(observations, new DateOnly(2024, 1, 2));

        Assert.Equal(new DateOnly(2024, 1, 2), result.Date);
        Assert.Equal(11.0m, result.Value);
    }

    [Fact]
    public void FindNearest_TargetBetweenDates_ReturnsClosest()
    {
        var observations = new List<RiksbankObservation>
        {
            new() { Date = new DateOnly(2024, 1, 1), Value = 10.0m },
            new() { Date = new DateOnly(2024, 1, 5), Value = 15.0m },
        };

        var result = NearestDateHelper.FindNearest(observations, new DateOnly(2024, 1, 4));

        Assert.Equal(new DateOnly(2024, 1, 5), result.Date);
    }

    [Fact]
    public void FindNearest_Equidistant_PrefersEarlierDate()
    {
        var observations = new List<RiksbankObservation>
        {
            new() { Date = new DateOnly(2024, 1, 1), Value = 10.0m },
            new() { Date = new DateOnly(2024, 1, 3), Value = 12.0m },
        };

        var result = NearestDateHelper.FindNearest(observations, new DateOnly(2024, 1, 2));

        Assert.Equal(new DateOnly(2024, 1, 1), result.Date);
    }

    [Fact]
    public void FindNearest_SingleObservation_ReturnsThatOne()
    {
        var observations = new List<RiksbankObservation>
        {
            new() { Date = new DateOnly(2024, 3, 15), Value = 9.5m },
        };

        var result = NearestDateHelper.FindNearest(observations, new DateOnly(2024, 1, 1));

        Assert.Equal(new DateOnly(2024, 3, 15), result.Date);
    }

    [Fact]
    public void FindNearest_EmptyList_ThrowsArgumentException()
    {
        var observations = new List<RiksbankObservation>();

        Assert.Throws<ArgumentException>(
            () => NearestDateHelper.FindNearest(observations, new DateOnly(2024, 1, 1)));
    }
}
