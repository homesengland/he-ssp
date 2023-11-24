namespace HE.Investments.Loans.Common.Extensions;

public static class DateTimeExtensions
{
    public static bool IsBeforeOrEqualTo(this DateTime date, DateTime otherDate) => date <= otherDate;

    public static bool IsAfter(this DateTime date, DateTime otherDate) => date > otherDate;

    public static DateTime ConvertUtcToUkLocalTime(this DateTime utcDate)
    {
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

        return timeZoneInfo.IsDaylightSavingTime(utcDate) ? utcDate.AddHours(1) : utcDate;
    }
}
