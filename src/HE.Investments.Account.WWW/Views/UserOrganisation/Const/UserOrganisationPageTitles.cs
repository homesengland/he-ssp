namespace HE.Investments.Account.WWW.Views.UserOrganisation.Const;

public static class UserOrganisationPageTitles
{
    public const string ChangeOrganisationDetails = "Request to change organisation details";

    public static string Details(string? organisationName) => $"Manage {organisationName ?? "organisation"} details";

    public static string OrganisationDetails(string organisationName) => $"{organisationName.ToUpperInvariant()}'s Homes England account";
}
