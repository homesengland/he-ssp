using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class AchievementDate : DateValueObject
{
    private const string FieldDescription = "achievement date";

    public AchievementDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, "AchievementDate", FieldDescription, !exists)
    {
    }

    public AchievementDate(DateTime? value)
        : base(value)
    {
    }

    public static AchievementDate FromDateDetails(DateDetails? date) =>
        new(date is { IsEmpty: false }, date?.Day, date?.Month, date?.Year);
}
