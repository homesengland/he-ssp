using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;

public class GrantDetails : ValueObject
{
    public GrantDetails(decimal totalGrantAllocated, decimal amountPaid, decimal amountRemaining)
    {
        TotalGrantAllocated = totalGrantAllocated;
        AmountPaid = amountPaid;
        AmountRemaining = amountRemaining;
    }

    public decimal TotalGrantAllocated { get; }

    public decimal AmountPaid { get; }

    public decimal AmountRemaining { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return TotalGrantAllocated;
        yield return AmountPaid;
        yield return AmountRemaining;
    }
}
