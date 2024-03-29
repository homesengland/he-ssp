using HE.Investments.Common.Domain;

namespace HE.Investments.Organisation.LocalAuthorities.ValueObjects;

public class LocalAuthorityCode : ValueObject
{
    public LocalAuthorityCode(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static LocalAuthorityCode From(string value) => new(value);

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
