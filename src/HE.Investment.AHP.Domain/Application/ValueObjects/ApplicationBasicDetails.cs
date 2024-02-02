using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationBasicDetails : ValueObject
{
    public ApplicationBasicDetails(AhpApplicationId id, ApplicationName name, ApplicationTenure? tenure, ApplicationStatus status)
    {
        Id = id;
        Name = name;
        Tenure = tenure;
        Status = status;
    }

    public AhpApplicationId Id { get; }

    public ApplicationName Name { get; }

    public ApplicationTenure? Tenure { get; }

    public ApplicationStatus Status { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return Tenure;
        yield return Status;
    }
}
