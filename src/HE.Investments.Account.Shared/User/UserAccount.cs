using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Loans.Common.Exceptions;

namespace HE.Investments.Account.Shared.User;

public record UserAccount(
    UserGlobalId UserGlobalId,
    string UserEmail,
    OrganisationId? OrganisationId,
    string OrganisationName,
    IReadOnlyCollection<UserRole> Roles)
{
    public UserRole Role() => Roles.Count > 0 ? Roles.Max() : throw new UnauthorizedAccessException();

    public bool CanViewAllApplications() => Role() != UserRole.Limited;

    public OrganisationId SelectedOrganisationId() => OrganisationId ?? throw new NotFoundException("User is not connected to any Organisation");

    public bool HasOneOfRole(UserRole[] roles)
    {
        return roles.Contains(Role());
    }
}
