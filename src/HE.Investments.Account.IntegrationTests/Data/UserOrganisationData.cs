using OrganisationShortId = HE.Investments.Common.Contract.OrganisationId;

namespace HE.Investments.Account.IntegrationTests.Data;

public class UserOrganisationData
{
    public string OrganisationName { get; private set; }

    public string OrganisationId { get; private set; }

    public void SetOrganisationName(string organisationName)
    {
        OrganisationName = organisationName;
    }

    public void SetOrganisationId(string organisationId)
    {
        OrganisationId = OrganisationShortId.From(organisationId).ToString();
    }
}
