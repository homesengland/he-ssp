namespace HE.Investments.Account.WWW.Views.UserOrganisation;

public static class UserOrganisationPageTitles
{
    public const string ChangeOrganisationDetails = "Request to change organisation details";

    public const string OrganisationsList = "Your organisations";

    public static string Details(string? organisationName) => $"Manage {organisationName ?? "organisation"} details";
}
