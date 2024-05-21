using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Project.ValueObjects;

public class AhpProjectApplication : ValueObject
{
    public AhpProjectApplication(
        AhpApplicationId id,
        ApplicationName name,
        ApplicationStatus applicationStatus,
        SchemeFunding funding,
        Tenure tenure)
    {
        Id = id;
        Name = name;
        ApplicationStatus = applicationStatus;
        Funding = funding;
        Tenure = tenure;
    }

    public AhpApplicationId Id { get; }

    public ApplicationName Name { get; }

    public ApplicationStatus ApplicationStatus { get; }

    public SchemeFunding Funding { get; }

    public Tenure Tenure { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return ApplicationStatus;
        yield return Funding;
        yield return Tenure;
    }
}
