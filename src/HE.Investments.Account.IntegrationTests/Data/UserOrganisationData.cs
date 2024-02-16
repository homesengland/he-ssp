namespace HE.Investments.Account.IntegrationTests.Data;

public class UserOrganisationData
{
    public string OrganisationName { get; private set; }

    public void SetOrganisationName(string organisationName)
    {
        OrganisationName = organisationName;
    }
}
