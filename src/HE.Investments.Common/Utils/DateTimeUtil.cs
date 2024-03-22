namespace HE.Investments.Common.Utils;

public static class DateTimeUtil
{
    private static IDateTimeProvider _dateTimeProvider = new DateTimeProvider();

    public static void SetDateTimeProvider(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public static bool IsDateWithinXYearsFromNow(DateTime? date, int yearsToCheck)
    {
        var timeNow = _dateTimeProvider.UtcNow;

        return date <= timeNow.AddYears(yearsToCheck) && date >= timeNow.AddYears(-yearsToCheck);
    }
}
