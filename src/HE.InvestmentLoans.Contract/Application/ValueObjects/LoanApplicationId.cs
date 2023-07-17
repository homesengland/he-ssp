using Dawn;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.BusinessLogic.Application.ValueObjects;

public class LoanApplicationId : ValueObject
{
    public LoanApplicationId(Guid value)
    {
        Value = Guard.Argument(value, nameof(LoanApplicationId)).NotDefault();
    }

    public Guid Value { get; }

    public static LoanApplicationId From(Guid value) => new(value);

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
