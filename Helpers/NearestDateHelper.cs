using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Helpers;

/// <summary>
/// Selects the observation whose date is closest to a given target date.
/// Used when a requested date falls on a non-bank day.
/// </summary>
public static class NearestDateHelper
{
    /// <summary>
    /// Returns the observation with the date nearest to <paramref name="targetDate"/>.
    /// If two dates are equidistant, the earlier one is preferred.
    /// </summary>
    public static RiksbankObservation FindNearest(IReadOnlyList<RiksbankObservation> observations, DateOnly targetDate)
    {
        if (observations.Count == 0)
            throw new ArgumentException("Observations list must not be empty.", nameof(observations));

        RiksbankObservation best = observations[0];
        int bestDistance = Math.Abs(best.Date.DayNumber - targetDate.DayNumber);

        for (int i = 1; i < observations.Count; i++)
        {
            int distance = Math.Abs(observations[i].Date.DayNumber - targetDate.DayNumber);
            if (distance < bestDistance)
            {
                best = observations[i];
                bestDistance = distance;
            }
        }

        return best;
    }
}
