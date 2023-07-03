namespace HE.InvestmentLoans.Common.Authorization;

public interface IUserContext
{
    public string UserGlobalId { get; }

    public IList<string> Roles { get; }
}
