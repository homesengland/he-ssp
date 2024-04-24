using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class AcquisitionDate : DateValueObject
{
    private const string FieldDescription = "acquisition date";

    public AcquisitionDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", FieldDescription, !exists)
    {
        Exists = exists;
    }

    private AcquisitionDate(bool exists, DateTime value)
        : base(value)
    {
        Exists = exists;
    }

    public new DateTime? Value => Exists ? base.Value : null;

    public bool Exists { get; }

    public static AcquisitionDate FromDateTime(DateTime? value) => new(value.HasValue, value ?? default);

    public static AcquisitionDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
        yield return Exists;
    }
}
