using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investments.AHP.Allocation.Domain.UserContext;

public class AllocationAccessContext : ConsortiumAccessContext, IAllocationAccessContext
{
    public const string ViewAllocation = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)},{nameof(UserRole.ViewOnly)}";

    public const string ManageAllocation = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)}";

    public const string ViewClaims = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)},{nameof(UserRole.ViewOnly)}";

    public const string FulfillClaims = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)}";

    public const string SubmitClaims = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)}";

    public static readonly IReadOnlyCollection<UserRole> FulfillClaimsRoles = ToUserAccountRoles(FulfillClaims);

    public static readonly IReadOnlyCollection<UserRole> SubmitClaimsRoles = ToUserAccountRoles(SubmitClaims);

    private readonly IConsortiumUserContext _consortiumUserContext;

    public AllocationAccessContext(IConsortiumUserContext consortiumUserContext)
        : base(consortiumUserContext)
    {
        _consortiumUserContext = consortiumUserContext;
    }

    public async Task<bool> CanEditClaim()
    {
        var account = await _consortiumUserContext.GetSelectedAccount();
        return account.HasOneOfRole(FulfillClaimsRoles) && account.IsLeadOrganisation;
    }

    public async Task<bool> CanSubmitClaim()
    {
        var account = await _consortiumUserContext.GetSelectedAccount();
        return account.HasOneOfRole(SubmitClaimsRoles) && account.IsLeadOrganisation;
    }
}
