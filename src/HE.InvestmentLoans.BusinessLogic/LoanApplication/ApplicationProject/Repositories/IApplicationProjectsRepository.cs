using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;

public interface IApplicationProjectsRepository
{
    public void AddAnotherProject(LoanApplicationEntity loanApplicationEntity, UserAccount userAccount);

    public void UpdateProject(LoanApplicationEntity loanApplicationEntity, Project project, UserAccount userAccount);

    public void DeleteProject(LoanApplicationEntity loanApplicationEntity, Guid projectId, UserAccount userAccount);

    public ApplicationProjects GetAllProjects(LoanApplicationId loanApplicationId, UserAccount userAccount);

    public Project GetProjectDetails(LoanApplicationId loanApplicationId, Guid projectId, UserAccount userAccount);
}
