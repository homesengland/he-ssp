namespace HE.Investments.Account.WWW.Views.Organisation;

public static class OrganisationPageTitles
{
    public const string SelectYourOrganisation = "Select your organisation";

    public const string ConfirmYourSelection = "Is this the correct organisation?";

    public const string SearchForYourOrganisation = "Search for your organisation";

    public const string NoMatch = "The details you entered did not match our records";

    public const string CreateOrganisation = "Enter your organisation details";

    public const string OrganisationName = "Organisation name";

    public const string OrganisationAddress = "Registered address";

    public static string OrganisationDashboard(string organisationName) => $"{organisationName}'s Homes England account";
}
