namespace HE.Investments.Account.WWW.Views.Organisation;

public static class OrganisationPageTitles
{
    public const string SelectYourOrganisation = "Select your organisation";

    public const string ConfirmYourSelection = "Confirm your selection";

    public const string SearchForYourOrganisation = "Your organisation details";

    public const string NoMatch = "The details you entered did not match our records";

    public const string CreateOrganisation = "Organisation Details";

    public static string OrganisationDashboard(string organisationName) => $"{organisationName}'s Homes England account";
}
