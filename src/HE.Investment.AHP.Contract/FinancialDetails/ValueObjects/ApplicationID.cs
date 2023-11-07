using Dawn;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
public class ApplicationID : ValueObject
{
    public ApplicationID(Guid value)
    {
        Value = Guard.Argument(value, nameof(ApplicationID)).NotDefault();
    }

    public Guid Value { get; }

    public static ApplicationID From(Guid value) => new(value);

    public static ApplicationID From(string value) => new(Guid.Parse(value));

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
