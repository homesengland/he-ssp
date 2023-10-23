using Dawn;
using HE.Investment.AHP.Contract.Domain;

namespace HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
public class FinancialDetailsId : ValueObject
{
    public FinancialDetailsId(Guid value)
    {
        Value = Guard.Argument(value, nameof(FinancialDetailsId)).NotDefault();
    }

    public Guid Value { get; }

    public static FinancialDetailsId From(Guid value) => new(value);

    public static FinancialDetailsId From(string value) => new(Guid.Parse(value));

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
