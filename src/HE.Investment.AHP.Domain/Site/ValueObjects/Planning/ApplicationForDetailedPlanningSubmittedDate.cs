using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class ApplicationForDetailedPlanningSubmittedDate : DateValueObject
{
    private const string FieldDescription = "application for detailed planning submitted date";

    public ApplicationForDetailedPlanningSubmittedDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, nameof(ApplicationForDetailedPlanningSubmittedDate), FieldDescription, !exists)
    {
        Exists = exists;
    }

    private ApplicationForDetailedPlanningSubmittedDate(bool exists, DateTime value)
        : base(value)
    {
        Exists = exists;
    }

    public new DateTime? Value => Exists ? base.Value : null;

    public bool Exists { get; }

    public static ApplicationForDetailedPlanningSubmittedDate FromCrm(DateTime? value) => new(value.HasValue, value ?? default);

    public static ApplicationForDetailedPlanningSubmittedDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value!;
        yield return Exists;
    }
}
