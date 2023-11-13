using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
public interface ILocalAuthorityRepository
{
    Task AssignLocalAuthority(LoanApplicationId loanApplicationId, ProjectId projectId, LocalAuthorityId localAuthorityId, CancellationToken cancellationToken);

    Task<(IList<LocalAuthority> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken);
}
