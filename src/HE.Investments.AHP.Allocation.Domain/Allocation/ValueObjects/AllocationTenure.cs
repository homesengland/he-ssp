using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;

public class AllocationTenure : ValueObject
{
    public AllocationTenure(Tenure value)
    {
        Value = value;
    }

    public Tenure Value { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
