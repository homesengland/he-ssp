using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class CompletionDate : DateValueObject
{
    private const string FieldDescription = "completion date";

    public CompletionDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", FieldDescription, !exists) //todo ms milestonestartat?
    {
        Exists = exists;
    }

    private CompletionDate(bool exists, DateTime value)
        : base(value)
    {
        Exists = exists;
    }

    public new DateTime? Value => Exists ? base.Value : null;

    public bool Exists { get; }

    public static CompletionDate FromCrm(DateTime? value) => new(value.HasValue, value ?? default);

    public static CompletionDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value!;
        yield return Exists;
    }
}
