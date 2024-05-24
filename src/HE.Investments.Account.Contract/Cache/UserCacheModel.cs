using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;

namespace HE.Investments.Account.Contract.Cache;

public record UserCacheModel(IList<UserAccount>? UserAccounts, UserProfileDetails? UserProfileDetails, AhpConsortiumBasicInfo? OrganisationConsortium);
