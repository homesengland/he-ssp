namespace HE.Investments.Common.Utils;

public static class DateTimeUtil
{
    public static bool IsDateWithinXYearsFromNow(string? date, int yearsToCheck)
    {
        if (DateTime.TryParse(date, out var parsedDate))
        {
            return parsedDate <= DateTime.Now.AddYears(yearsToCheck) && parsedDate >= DateTime.Now.AddYears(-yearsToCheck);
        }

        return false;
    }
}
