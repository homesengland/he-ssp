using System.Globalization;
using HE.Investments.Common.Contract;

namespace HE.Investments.Common.Extensions;

public static class DateTimeExtensions
{
    public static bool IsBeforeOrEqualTo(this DateTime date, DateTime otherDate) => date <= otherDate;

    public static bool IsBefore(this DateTime date, DateTime otherDate) => date < otherDate;

    public static bool IsAfter(this DateTime date, DateTime otherDate) => date > otherDate;

    public static DateTime ConvertUtcToUkLocalTime(this DateTime utcDate)
    {
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

        return timeZoneInfo.IsDaylightSavingTime(utcDate) ? utcDate.AddHours(1) : utcDate;
    }

    public static DateTime? FromDateDetails(DateDetails? dateDetails)
    {
        if (dateDetails == null || dateDetails.IsEmpty)
        {
            return null;
        }

        var day = int.Parse(dateDetails.Day!, CultureInfo.InvariantCulture);
        var month = int.Parse(dateDetails.Month!, CultureInfo.InvariantCulture);
        var year = int.Parse(dateDetails.Year!, CultureInfo.InvariantCulture);

        return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
    }
}
