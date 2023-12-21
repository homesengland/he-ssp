using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;

namespace HE.Investments.Account.Shared.Config;

internal static class CacheKeys
{
    public static string UserAccounts(string userGlobalId) => $"{nameof(UserAccount)}-{userGlobalId}";

    public static string ProfileDetails(string userGlobalId) => $"{nameof(UserProfileDetails)}-{userGlobalId}";
}
