using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
public interface ILocalAuthorityRepository
{
    Task AssignLocalAuthority(LoanApplicationId loanApplicationId, ProjectId projectId, LocalAuthorityId localAuthorityId, CancellationToken cancellationToken);

    Task<(IEnumerable<LocalAuthorityTemporaryDto> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken);
}
