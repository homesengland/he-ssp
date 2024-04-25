using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Consortium.Contract;

public class ConsortiumMember : ValueObject
{
    public ConsortiumMember(OrganisationId id, string organisationName)
    {
        if (string.IsNullOrWhiteSpace(organisationName))
        {
            throw new ArgumentException("Organisation Name cannot be empty");
        }

        Id = id;
        OrganisationName = organisationName;
    }

    public OrganisationId Id { get; }

    public string OrganisationName { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return OrganisationName;
    }
}