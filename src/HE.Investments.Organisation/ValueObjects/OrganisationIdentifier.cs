using HE.Investments.Common.Contract;

namespace HE.Investments.Organisation.ValueObjects;

public record OrganisationIdentifier : StringIdValueObject
{
    public OrganisationIdentifier(string organisationIdOrCompanyHouseNumber)
        : base(organisationIdOrCompanyHouseNumber)
    {
    }
}
