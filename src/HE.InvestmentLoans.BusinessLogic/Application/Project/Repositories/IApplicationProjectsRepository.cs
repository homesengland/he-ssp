using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System;

namespace HE.InvestmentLoans.BusinessLogic.Application.Project.Repositories;

public interface IApplicationProjectsRepository
{
    public LoanApplicationViewModel DeleteProject(Guid loanApplicationId, Guid projectId);
}
