using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class MilestonePaymentDate : DateValueObject
{
    private const string FieldDescription = "milestone payment date";

    public MilestonePaymentDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, "ClaimMilestonePaymentAt", FieldDescription, !exists)
    {
        Exists = exists;
    }

    private MilestonePaymentDate(bool exists, DateTime value)
        : base(value)
    {
        Exists = exists;
    }

    public new DateTime? Value => Exists ? base.Value : null;

    public bool Exists { get; }

    public static MilestonePaymentDate FromDateTime(DateTime? value) => new(value.HasValue, value ?? default);

    public static MilestonePaymentDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
        yield return Exists;
    }
}
