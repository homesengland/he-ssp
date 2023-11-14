using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
public interface ILocalAuthorityRepository
{
    Task AssignLocalAuthority(LoanApplicationId loanApplicationId, ProjectId projectId, LocalAuthority localAuthority, CancellationToken cancellationToken);

    Task<(IList<LocalAuthority> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken);
}
