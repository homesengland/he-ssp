using HE.Investments.Common.Domain;

namespace HE.Investments.Organisation.LocalAuthorities.ValueObjects;

public class LocalAuthority : ValueObject
{
    public LocalAuthority(LocalAuthorityCode code, string name)
    {
        Code = code;
        Name = name;
    }

    public LocalAuthorityCode Code { get; }

    public string Name { get; }

    public static LocalAuthority New(string code, string name) => new(LocalAuthorityCode.From(code), name);

    public static LocalAuthority New(LocalAuthorityCode code, string name) => new(code, name);

    public override string ToString()
    {
        return Name;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Code;
        yield return Name;
    }
}
