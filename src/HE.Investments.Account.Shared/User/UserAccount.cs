using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investments.Account.Shared.User;

public record UserAccount(
    UserGlobalId UserGlobalId,
    string UserEmail,
    OrganisationBasicInfo? Organisation,
    IReadOnlyCollection<UserRole> Roles) : IUserAccount
{
    public bool CanManageUsers => HasOneOfRole([.. AccountAccessContext.ManageUsersRoles]);

    public bool CanAccessOrganisationView => HasOneOfRole([.. AccountAccessContext.OrganisationViewRoles]);

    public bool CanSubmitApplication => HasOneOfRole([.. AccountAccessContext.SubmitApplicationRoles]);

    public bool CanEditApplication => HasOneOfRole([.. AccountAccessContext.EditApplicationRoles]);

    public UserRole Role() => Roles.Count > 0 ? Roles.Max() : throw new UnauthorizedAccessException();

    public bool CanViewAllApplications() => Role() != UserRole.Limited;

    public OrganisationId SelectedOrganisationId() => Organisation == null ? throw new NotFoundException("User is not connected to any Organisation") : Organisation.OrganisationId;

    public OrganisationBasicInfo SelectedOrganisation() => Organisation ?? throw new NotFoundException("User is not connected to any Organisation");

    private bool HasOneOfRole(IEnumerable<UserRole> roles)
    {
        return roles.Contains(Role());
    }
}
