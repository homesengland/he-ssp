using HE.Investments.Account.Contract.Users;
using HE.Investments.Loans.Common.Exceptions;

namespace HE.Investments.Account.Shared.User;

public record UserAccount(
    UserGlobalId UserGlobalId,
    string UserEmail,
    Guid? AccountId,
    string AccountName,
    IReadOnlyCollection<UserRole> Roles)
{
    public UserRole Role() => Roles.Count > 0 ? Roles.Max() : throw new UnauthorizedAccessException();

    public Guid OrganisationId() => AccountId ?? throw new NotFoundException("User is not connected to any Organisation");

    public bool HasOneOfRole(UserRole[] roles)
    {
        return roles.Contains(Role());
    }
}
