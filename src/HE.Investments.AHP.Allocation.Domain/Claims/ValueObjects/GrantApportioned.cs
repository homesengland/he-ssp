using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class GrantApportioned : ValueObject
{
    public GrantApportioned(decimal amount, decimal percentage)
    {
        Amount = amount;
        Percentage = percentage;
    }

    public decimal Amount { get; }

    public decimal Percentage { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Amount;
        yield return Percentage;
    }
}
