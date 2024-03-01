namespace HE.Investments.Account.IntegrationTests.Data;

public class FreshProfileData
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string JobTitle { get; private set; }

    public string TelephoneNumber { get; private set; }

    public string OrganisationName { get; private set; }

    public string SelectedOrganisationId { get; private set; }

    public FreshProfileData GenerateProfileData()
    {
        FirstName = "John";
        LastName = "Smith";
        JobTitle = "Software Developer";
        TelephoneNumber = "+44 (0)20 7450 4000";
        return this;
    }

    public FreshProfileData GenerateOrganisationSearch()
    {
        OrganisationName = "HOMES OF ENGLAND LIMITED";
        return this;
    }

    public void SetSelectedOrganisationId(string organisationId)
    {
        SelectedOrganisationId = organisationId;
    }
}
