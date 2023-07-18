namespace HE.InvestmentLoans.BusinessLogic.User;

public record UserAccount(string UserGlobalId, Guid AccountId, string Name)
{
    public UserAccount(string userGlobalId, Guid accountId)
        : this(userGlobalId, accountId, string.Empty)
    {
    }
}
