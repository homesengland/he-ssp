using HE.Investments.Common.Domain;

namespace HE.Investments.Organisation.LocalAuthorities.ValueObjects;

public class LocalAuthority : ValueObject
{
    public LocalAuthority(LocalAuthorityId id, string name)
    {
        Id = id;
        Name = name;
    }

    public LocalAuthorityId Id { get; }

    public string Name { get; }

    public static LocalAuthority New(string id, string name) => new(LocalAuthorityId.From(id), name);

    public static LocalAuthority New(LocalAuthorityId id, string name) => new(id, name);

    public override string ToString()
    {
        return Name;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
    }
}
