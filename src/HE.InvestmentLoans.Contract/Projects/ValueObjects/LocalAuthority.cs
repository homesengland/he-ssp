using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.Contract.Projects.ValueObjects;

public class LocalAuthority : ValueObject
{
    public LocalAuthority(string id, string name)
    {
        Id = LocalAuthorityId.From(id);
        Name = name;
    }

    public LocalAuthorityId Id { get; }

    public string Name { get; }

    public static LocalAuthority New(string id, string name) => new(id, name);

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
