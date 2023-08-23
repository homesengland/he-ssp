using Dawn;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;
public class UserGlobalId : ValueObject
{
    public UserGlobalId(string value)
    {
        Value = Guard.Argument(value, nameof(UserGlobalId));
    }

    public string Value { get; }

    public static UserGlobalId From(string value) => new(value);

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
