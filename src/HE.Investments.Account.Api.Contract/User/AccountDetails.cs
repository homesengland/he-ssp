namespace HE.Investments.Account.Api.Contract.User;

public record AccountDetails(string UserGlobalId, string UserEmail, IReadOnlyCollection<UserOrganisation> Organisations);
