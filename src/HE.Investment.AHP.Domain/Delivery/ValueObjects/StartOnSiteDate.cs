using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class StartOnSiteDate : DateValueObject
{
    private const string FieldDescription = "start on site date";

    public StartOnSiteDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", FieldDescription, !exists)
    {
        Exists = exists;
    }

    private StartOnSiteDate(bool exists, DateTime value)
        : base(value)
    {
        Exists = exists;
    }

    public new DateTime? Value => Exists ? base.Value : null;

    public bool Exists { get; }

    public static StartOnSiteDate FromDateTime(DateTime? value) => new(value.HasValue, value ?? default);

    public static StartOnSiteDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
        yield return Exists;
    }
}
