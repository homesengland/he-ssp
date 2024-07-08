using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class BuildActivity : ValueObject
{
    public BuildActivity(BuildActivityType value)
    {
        Value = value;
    }

    public BuildActivityType Value { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
