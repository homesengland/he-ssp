using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class NumberOfHomes : ValueObject
{
    public NumberOfHomes(int value)
    {
        Value = value;
    }

    public int Value { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
