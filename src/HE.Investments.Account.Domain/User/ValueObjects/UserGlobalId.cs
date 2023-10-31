using HE.Investments.Common.Domain;

namespace HE.Investments.Account.Domain.User.ValueObjects;

public class UserGlobalId : ValueObject
{
    public UserGlobalId(string value)
    {
        Value = value;
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
