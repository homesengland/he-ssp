namespace HE.InvestmentLoans.BusinessLogic.User.Entities;

public record UserAccount(string UserGlobalId, string UserEmail, Guid AccountId, string AccountName)
{
}
