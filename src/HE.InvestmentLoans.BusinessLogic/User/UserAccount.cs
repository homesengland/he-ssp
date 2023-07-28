namespace HE.InvestmentLoans.BusinessLogic.User;

public record UserAccount(string UserGlobalId, string UserEmail, Guid AccountId, string AccountName)
{
}
