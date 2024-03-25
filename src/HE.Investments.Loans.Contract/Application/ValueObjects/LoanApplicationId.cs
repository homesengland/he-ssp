using HE.Investments.Common.Domain;

namespace HE.Investments.Loans.Contract.Application.ValueObjects;

public class LoanApplicationId : ValueObject
{
    public LoanApplicationId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static LoanApplicationId From(Guid value) => new(value);

    public static LoanApplicationId From(string value) => new(Guid.Parse(value));

    public static LoanApplicationId New() => new(Guid.Empty);

    public bool IsSaved() => Value != Guid.Empty;

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
