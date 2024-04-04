namespace HE.Investments.Common.Utils;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

    public DateTime UtcNow => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
}
