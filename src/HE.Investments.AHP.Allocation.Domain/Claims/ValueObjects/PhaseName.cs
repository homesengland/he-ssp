using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class PhaseName : ValueObject
{
    public PhaseName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
