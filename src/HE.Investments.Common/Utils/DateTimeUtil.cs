using System.Globalization;

namespace HE.Investments.Common.Utils;

public static class DateTimeUtil
{
    private static IDateTimeProvider _dateTimeProvider = new DateTimeProvider();

    public static void SetDateTimeProvider(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public static bool IsDateWithinXYearsFromNow(string? date, int yearsToCheck)
    {
        var timeNow = _dateTimeProvider.UtcNow;

        if (DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            return parsedDate <= timeNow.AddYears(yearsToCheck) && parsedDate >= timeNow.AddYears(-yearsToCheck);
        }

        return false;
    }
}
