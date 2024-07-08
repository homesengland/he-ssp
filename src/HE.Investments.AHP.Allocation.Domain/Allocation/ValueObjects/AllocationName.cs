using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;

public class AllocationName : ValueObject
{
    public AllocationName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
