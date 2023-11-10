using Dawn;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ApplicationId : ValueObject
{
    public ApplicationId(Guid value)
    {
        Value = Guard.Argument(value, nameof(ApplicationId)).NotDefault();
    }

    public Guid Value { get; }

    public static ApplicationId From(Guid value) => new(value);

    public static ApplicationId From(string value) => new(Guid.Parse(value));

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
