using OrganisationShortId = HE.Investments.Common.Contract.OrganisationId;

namespace HE.Investments.Account.IntegrationTests.Data;

public class UserOrganisationsData
{
    public string MainOrganisationName { get; private set; }

    public string MainOrganisationId { get; private set; }

    public string SecondaryOrganisationName { get; private set; }

    public string SecondaryOrganisationId { get; private set; }

    public void SetOrganisationName(string organisationName)
    {
        MainOrganisationName = organisationName;
    }

    public void SetSecondOrganisationName(string secondOrganisationName)
    {
        SecondaryOrganisationName = secondOrganisationName;
    }

    public void SetOrganisationId(string organisationId)
    {
        MainOrganisationId = OrganisationShortId.From(organisationId).ToString();
    }

    public void SetSecondOrganisationId(string secondOrganisationId)
    {
        SecondaryOrganisationId = OrganisationShortId.From(secondOrganisationId).ToString();
    }
}
