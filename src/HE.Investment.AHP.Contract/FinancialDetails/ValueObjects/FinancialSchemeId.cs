using Dawn;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
public class FinancialSchemeId : ValueObject
{
    public FinancialSchemeId(Guid value)
    {
        Value = Guard.Argument(value, nameof(FinancialSchemeId)).NotDefault();
    }

    public Guid Value { get; }

    public static FinancialSchemeId From(Guid value) => new(value);

    public static FinancialSchemeId From(string value) => new(Guid.Parse(value));

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
