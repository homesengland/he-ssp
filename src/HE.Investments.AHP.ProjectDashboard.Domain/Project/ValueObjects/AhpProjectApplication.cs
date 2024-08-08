using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;

public class AhpProjectApplication : ValueObject
{
    public AhpProjectApplication(
        AhpApplicationId id,
        string name,
        ApplicationStatus applicationStatus,
        int requiredFunding,
        int housedToDeliver,
        Tenure tenure,
        DateTime? lastModificationOn,
        string? localAuthorityName)
    {
        Id = id;
        Name = name;
        ApplicationStatus = applicationStatus;
        RequiredFunding = requiredFunding;
        HousesToDeliver = housedToDeliver;
        Tenure = tenure;
        LastModificationOn = lastModificationOn;
        LocalAuthorityName = localAuthorityName;
    }

    public AhpApplicationId Id { get; }

    public string Name { get; }

    public ApplicationStatus ApplicationStatus { get; }

    public int RequiredFunding { get; }

    public int HousesToDeliver { get; }

    public Tenure Tenure { get; }

    public DateTime? LastModificationOn { get; }

    public string? LocalAuthorityName { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return ApplicationStatus;
        yield return RequiredFunding;
        yield return HousesToDeliver;
        yield return Tenure;
        yield return LastModificationOn;
    }
}
