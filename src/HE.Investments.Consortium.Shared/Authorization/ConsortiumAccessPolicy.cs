using System.Diagnostics.CodeAnalysis;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investments.Consortium.Shared.Authorization;

public class ConsortiumAccessPolicy : IAccessPolicy
{
    private readonly IConsortiumUserContext _consortiumUserContext;

    public ConsortiumAccessPolicy(IConsortiumUserContext consortiumUserContext)
    {
        _consortiumUserContext = consortiumUserContext;
    }

    public async Task<bool> CanAccess(IList<UserRole> allowedFor)
    {
        var account = await _consortiumUserContext.GetSelectedAccount();
        var consortium = account.Consortium;

        var effectiveRoles = consortium.HasConsortium && consortium.IsNotLeadPartner
            ? AccountAccessContext.ViewOnlyRoles
            : account.Roles;

        return allowedFor.Any(allowedRole => effectiveRoles.Any(role => role == allowedRole));
    }
}
