using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;

namespace HE.Investments.Consortium.Shared.UserContext;

public record ConsortiumUserAccount(
    UserGlobalId UserGlobalId,
    string UserEmail,
    OrganisationBasicInfo? Organisation,
    IReadOnlyCollection<UserRole> Roles,
    ConsortiumBasicInfo Consortium)
    : UserAccount(UserGlobalId, UserEmail, Organisation, Roles), IConsortiumUserAccount
{
    public ConsortiumUserAccount(UserAccount userAccount, ConsortiumBasicInfo consortium)
        : this(userAccount.UserGlobalId, userAccount.UserEmail, userAccount.Organisation, userAccount.Roles, consortium)
    {
    }

    public bool CanManageConsortium => HasOneOfRole([.. ConsortiumAccessContext.ManageConsortiumRoles]);

    public bool CanEdit => CanEditApplication && IsLeadOrganisation;

    public bool CanSubmit => CanSubmitApplication && IsLeadOrganisation;

    public bool IsLeadOrganisation => Consortium.IsLeadPartner || Consortium.HasNoConsortium;
}
