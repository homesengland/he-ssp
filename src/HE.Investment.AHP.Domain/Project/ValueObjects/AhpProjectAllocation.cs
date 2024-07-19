using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Project.ValueObjects;

public class AhpProjectAllocation : ValueObject
{
    public AhpProjectAllocation(
        string id,
        string name,
        int housesToDeliver,
        Tenure tenure,
        string localAuthorityName,
        DateTime? lastModificationOn)
    {
        Id = id;
        Name = name;
        HousesToDeliver = housesToDeliver;
        Tenure = tenure;
        LocalAuthorityName = localAuthorityName;
        LastModificationOn = lastModificationOn;
    }

    public string Id { get; }

    public string Name { get; }

    public int HousesToDeliver { get; }

    public Tenure Tenure { get; }

    public string LocalAuthorityName { get; }

    public DateTime? LastModificationOn { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return HousesToDeliver;
        yield return Tenure;
        yield return LastModificationOn;
    }
}
