using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Shared.Config;

public static class AccountCacheKeys
{
    public static string UserAccounts(string userGlobalId) => $"{nameof(UserAccount)}-{userGlobalId}";

    public static string ProfileDetails(string userGlobalId) => $"{nameof(UserProfileDetails)}-{userGlobalId}";

    public static string OrganisationConsortium(OrganisationId organisationId) => $"ahp-consortium-{organisationId}";
}
