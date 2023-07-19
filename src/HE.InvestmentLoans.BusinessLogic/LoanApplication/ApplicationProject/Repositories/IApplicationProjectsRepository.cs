using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;

public interface IApplicationProjectsRepository
{
    public void Add(ApplicationProjects applicationProjects);

    public void Update(ApplicationProjects applicationProjects, Project project);

    public void Delete(ApplicationProjects applicationProjects, ProjectId projectId);

    public ApplicationProjects GetAll(LoanApplicationId loanApplicationId, UserAccount userAccount);

    public Project GetById(LoanApplicationId loanApplicationId, ProjectId projectId, UserAccount userAccount);

    public void Save(ApplicationProjects applicationProjects);
}
