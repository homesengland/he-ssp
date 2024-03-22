namespace HE.Investments.FrontDoor.Common.Extensions;

public static class DateOnlyExtensions
{
    public static DateTime? ToDateTime(this DateOnly? date)
    {
        return date?.ToDateTime(TimeOnly.MinValue);
    }
}
