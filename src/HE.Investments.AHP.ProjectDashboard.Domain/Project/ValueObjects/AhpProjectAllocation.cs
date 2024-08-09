using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;

public class AhpProjectAllocation : ValueObject
{
    public AhpProjectAllocation(
        string id,
        string name,
        int housesToDeliver,
        Tenure tenure,
        string localAuthorityName,
        DateTime? lastModificationOn,
        bool? hasMilestoneInDueState = null)
    {
        Id = id;
        Name = name;
        HousesToDeliver = housesToDeliver;
        Tenure = tenure;
        LocalAuthorityName = localAuthorityName;
        LastModificationOn = lastModificationOn;
        HasMilestoneInDueState = hasMilestoneInDueState;
    }

    public string Id { get; }

    public string Name { get; }

    public int HousesToDeliver { get; }

    public Tenure Tenure { get; }

    public string LocalAuthorityName { get; }

    public DateTime? LastModificationOn { get; }

    public bool? HasMilestoneInDueState { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return HousesToDeliver;
        yield return Tenure;
        yield return LastModificationOn;
    }
}
