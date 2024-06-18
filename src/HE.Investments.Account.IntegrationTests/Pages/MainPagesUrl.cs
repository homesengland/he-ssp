using System.Web;

namespace HE.Investments.Account.IntegrationTests.Pages;

public static class MainPagesUrl
{
    public const string MainPage = "/";

    public const string ManageUsers = "/users";

    public const string OrganisationList = "/user-organisations/list";

    public const string ProfileDetails = "/user/profile-details";

    public const string UserOrganisationDetails = "/user-organisation/details";

    public const string ChangeOrganisationDetails = "/user-organisation/request-details-change";

    public const string OrganisationSearch = "/organisation/search";

    public static string Dashboard(string organisationId) => $"{organisationId}/user-organisation";

    public static string ManageUser(string userGlobalId, string organisationId) => $"{organisationId}/users/{HttpUtility.UrlEncode(userGlobalId)}/manage";
}
