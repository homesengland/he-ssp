using System.Globalization;

namespace HE.Investments.Common.Contract;

public record DateDetails(string? Day, string? Month, string? Year)
{
    public bool IsEmpty => string.IsNullOrEmpty(Day) && string.IsNullOrEmpty(Month) && string.IsNullOrEmpty(Year);

    public static DateDetails Empty() => new(string.Empty, string.Empty, string.Empty);

    public static DateDetails? FromDateTime(DateTime? dateTime) => dateTime == null
        ? null
        : new DateDetails(
            dateTime.Value.Day.ToString(CultureInfo.InvariantCulture),
            dateTime.Value.Month.ToString(CultureInfo.InvariantCulture),
            dateTime.Value.Year.ToString(CultureInfo.InvariantCulture));

    public string ToFormattedDateString()
    {
        var culture = new CultureInfo("en-US");
        var monthName = Month != null ? culture.DateTimeFormat.GetMonthName(int.Parse(Month, CultureInfo.InvariantCulture)) : null;
        return $"{Day} {monthName} {Year}";
    }
}
