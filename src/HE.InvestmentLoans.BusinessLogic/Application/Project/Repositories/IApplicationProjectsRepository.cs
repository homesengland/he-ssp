using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic.Application.Project.Repositories;

public interface IApplicationProjectsRepository
{
    public LoanApplicationViewModel DeleteProject(Guid loanApplicationId, Guid projectId);
}
