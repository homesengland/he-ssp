using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Contract;

public record OrganisationDetails(string Name, string Street, string City, string PostalCode, string? CompanyHouseNumber, string? OrganisationId)
{
    public static OrganisationDetails WithoutAddress(OrganisationId organisationId, string organisationName)
    {
        return new OrganisationDetails(organisationName, string.Empty, string.Empty, string.Empty, string.Empty, organisationId.ToString());
    }
}
