using System.Globalization;

namespace HE.Investment.AHP.Common.Utils;

public static class CalculationUtilities
{
    public static decimal CalculateAffordableRent(string? homeWeeklyRent, string? affordableWeeklyRent)
    {
        var result = 00.00m;

        if (decimal.TryParse(homeWeeklyRent!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedHomeWeeklyRent)
            && decimal.TryParse(affordableWeeklyRent!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedAffordableWeeklyRent)
            && decimal.Round(parsedHomeWeeklyRent, 2) == parsedHomeWeeklyRent
            && decimal.Round(parsedAffordableWeeklyRent, 2) == parsedAffordableWeeklyRent)
        {
            result = parsedAffordableWeeklyRent / parsedHomeWeeklyRent * 100;
            result = Math.Round(result, 2);
        }

        return result;
    }
}
