namespace HE.Investments.Common.Utils;

public interface IDateTimeProvider
{
    public DateTime Now { get; }

    public DateTime UtcNow { get; }
}
