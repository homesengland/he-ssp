using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.UserContext;

public record AhpUserAccount(
    UserGlobalId UserGlobalId,
    string UserEmail,
    OrganisationBasicInfo? Organisation,
    IReadOnlyCollection<UserRole> Roles,
    AhpConsortiumBasicInfo Consortium)
    : UserAccount(UserGlobalId, UserEmail, Organisation, Roles)
{
    public AhpUserAccount(UserAccount userAccount, AhpConsortiumBasicInfo consortium)
        : this(userAccount.UserGlobalId, userAccount.UserEmail, userAccount.Organisation, userAccount.Roles, consortium)
    {
    }
}
