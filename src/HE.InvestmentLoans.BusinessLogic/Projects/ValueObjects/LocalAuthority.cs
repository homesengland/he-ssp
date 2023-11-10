using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;

public class LocalAuthority : ValueObject
{
    public LocalAuthority(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; }

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
