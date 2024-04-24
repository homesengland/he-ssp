using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class OutlinePlanningApprovalDate : DateValueObject
{
    private const string FieldDescription = "outline planning approval date";

    public OutlinePlanningApprovalDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, nameof(OutlinePlanningApprovalDate), FieldDescription, !exists)
    {
        Exists = exists;
    }

    private OutlinePlanningApprovalDate(bool exists, DateTime value)
        : base(value)
    {
        Exists = exists;
    }

    public new DateTime? Value => Exists ? base.Value : null;

    public bool Exists { get; }

    public static OutlinePlanningApprovalDate FromDateTime(DateTime? value) => new(value.HasValue, value ?? default);

    public static OutlinePlanningApprovalDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value!;
        yield return Exists;
    }
}
