using HE.Investments.Loans.Contract.Projects.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories;
public interface ILocalAuthorityRepository
{
    Task<(IList<LocalAuthority> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken);
}
