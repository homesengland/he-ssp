namespace HE.Investments.Account.Shared.User;

public record UserAccount(
    UserGlobalId UserGlobalId,
    string UserEmail,
    Guid? AccountId,
    string AccountName,
    IReadOnlyCollection<UserAccountRole> Roles)
{
    public UserAccountRole Role() => Roles.FirstOrDefault() ?? throw new UnauthorizedAccessException();
}
