using HE.Investments.Account.Domain.Organisation.Entities;

namespace HE.Investments.Account.Shared.User;

public record UserAccount(
    UserGlobalId UserGlobalId,
    string UserEmail,
    Guid? AccountId,
    string AccountName,
    IEnumerable<UserAccountRole> Roles);
