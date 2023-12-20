using HE.Investments.Account.Contract.Users;

namespace HE.Investments.Account.Shared.User;

public record UserAccount(
    UserGlobalId UserGlobalId,
    string UserEmail,
    Guid? AccountId,
    string AccountName,
    IReadOnlyCollection<UserRole> Roles)
{
    public UserRole Role() => Roles.Count > 0 ? Roles.FirstOrDefault() : throw new UnauthorizedAccessException();
}
