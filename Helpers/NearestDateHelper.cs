using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Helpers;

// Hittar observationen med datum närmast måldatumet
public static class NearestDateHelper
{
    // Returnerar observationen med närmaste datum till targetDate
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
