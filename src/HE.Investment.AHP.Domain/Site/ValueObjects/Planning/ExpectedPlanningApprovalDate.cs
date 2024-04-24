using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class ExpectedPlanningApprovalDate : DateValueObject
{
    private const string FieldDescription = "expected planning approval date";

    public ExpectedPlanningApprovalDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, nameof(ExpectedPlanningApprovalDate), FieldDescription, !exists)
    {
        Exists = exists;
    }

    private ExpectedPlanningApprovalDate(bool exists, DateTime value)
        : base(value)
    {
        Exists = exists;
    }

    public new DateTime? Value => Exists ? base.Value : null;

    public bool Exists { get; }

    public static ExpectedPlanningApprovalDate FromDateTime(DateTime? value) => new(value.HasValue, value ?? default);

    public static ExpectedPlanningApprovalDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value!;
        yield return Exists;
    }
}
