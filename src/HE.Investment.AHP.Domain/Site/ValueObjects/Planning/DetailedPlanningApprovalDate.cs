using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class DetailedPlanningApprovalDate : DateValueObject
{
    private const string FieldDescription = "detailed planning approval was granted date";

    public DetailedPlanningApprovalDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, nameof(DetailedPlanningApprovalDate), FieldDescription, !exists)
    {
        Exists = exists;
    }

    private DetailedPlanningApprovalDate(bool exists, DateTime value)
        : base(value)
    {
        Exists = exists;
    }

    public new DateTime? Value => Exists ? base.Value : null;

    public bool Exists { get; }

    public static DetailedPlanningApprovalDate FromCrm(DateTime? value) => new(value.HasValue, value ?? default);

    public static DetailedPlanningApprovalDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value!;
        yield return Exists;
    }
}
