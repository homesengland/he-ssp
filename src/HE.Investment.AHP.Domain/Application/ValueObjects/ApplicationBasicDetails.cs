using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationBasicDetails : ValueObject
{
    public ApplicationBasicDetails(ApplicationId id, ApplicationName name)
    {
        Id = id;
        Name = name;
    }

    public ApplicationId Id { get; }

    public ApplicationName Name { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
    }
}
