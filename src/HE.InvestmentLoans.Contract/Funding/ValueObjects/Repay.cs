using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.Contract.Funding.ValueObjects;
public class Repay : ValueObject
{
    public Repay(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Repay New(string value) => new(value);

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
