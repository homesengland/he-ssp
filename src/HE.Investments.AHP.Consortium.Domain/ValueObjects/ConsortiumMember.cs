using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Consortium.Domain.ValueObjects;

public class ConsortiumMember : ValueObject
{
    public ConsortiumMember(OrganisationId id, string organisationName, ConsortiumMemberStatus status)
    {
        if (string.IsNullOrWhiteSpace(organisationName))
        {
            throw new ArgumentException("Organisation Name cannot be empty");
        }

        Id = id;
        OrganisationName = organisationName;
        Status = status;
    }

    public OrganisationId Id { get; }

    public string OrganisationName { get; }

    public ConsortiumMemberStatus Status { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return OrganisationName;
        yield return Status;
    }
}
