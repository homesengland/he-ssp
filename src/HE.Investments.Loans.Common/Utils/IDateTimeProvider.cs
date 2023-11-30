namespace HE.Investments.Loans.Common.Utils;

public interface IDateTimeProvider
{
    public DateTime Now { get; }

    public DateTime UtcNow { get; }
}
