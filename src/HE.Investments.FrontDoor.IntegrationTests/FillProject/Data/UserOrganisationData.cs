using OrganisationShortId = HE.Investments.Common.Contract.OrganisationId;

namespace HE.Investments.FrontDoor.IntegrationTests.FillProject.Data;

public class UserOrganisationData
{
    public string OrganisationId { get; private set; }

    public void SetOrganisationId(string organisationId)
    {
        OrganisationId = OrganisationShortId.From(organisationId).ToString();
    }
}
