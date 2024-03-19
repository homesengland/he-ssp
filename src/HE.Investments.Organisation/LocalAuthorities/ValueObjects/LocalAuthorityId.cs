using HE.Investments.Common.Domain;

namespace HE.Investments.Organisation.LocalAuthorities.ValueObjects;

public class LocalAuthorityId : ValueObject
{
    public LocalAuthorityId(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static LocalAuthorityId From(string value) => new(value);

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
