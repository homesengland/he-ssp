using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

public class HomesBuilt : ValueObject
{
    public HomesBuilt(int value)
    {
        Value = value;
    }

    public int Value { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
