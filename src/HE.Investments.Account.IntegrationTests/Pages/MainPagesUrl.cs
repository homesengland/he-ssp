using System.Web;

namespace HE.Investments.Account.IntegrationTests.Pages;

public static class MainPagesUrl
{
    public const string MainPage = "/";

    public const string ManageUsers = "/users";

    public const string Dashboard = "/user-organisation";

    public const string OrganisationList = "/user-organisation/list";

    public const string ProfileDetails = "/user/profile-details";

    public const string UserOrganisationDetails = "/user-organisation/details";

    public const string ChangeOrganisationDetails = "/user-organisation/request-details-change";

    public static string ManageUser(string userGlobalId) => $"users/{HttpUtility.UrlEncode(userGlobalId)}/manage";
}
