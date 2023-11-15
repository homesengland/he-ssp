using HE.InvestmentLoans.Contract.Projects.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
public interface ILocalAuthorityRepository
{
    Task<(IList<LocalAuthority> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken);
}
