using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;

public interface IApplicationProjectsRepository
{
    public ApplicationProjects GetAll(LoanApplicationId loanApplicationId, UserAccount userAccount);

    public Project GetById(LoanApplicationId loanApplicationId, ProjectId projectId, UserAccount userAccount);

    public void Save(ApplicationProjects applicationProjects);

    public LoanApplicationViewModel LegacyDeleteProject(Guid loanApplicationId, Guid projectId);
}
