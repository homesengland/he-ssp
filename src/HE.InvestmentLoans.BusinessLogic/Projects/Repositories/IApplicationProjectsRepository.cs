using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Shared.User;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;

public interface IApplicationProjectsRepository
{
    Task SaveAsync(LoanApplicationId loanApplicationId, Project projectToSave, CancellationToken cancellationToken);

    Task<ApplicationProjects> GetAllAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<Project> GetByIdAsync(ProjectId projectId, UserAccount userAccount, ProjectFieldsSet projectFieldsSet, CancellationToken cancellationToken);

    Task SaveAllAsync(ApplicationProjects applicationProjects, UserAccount userAccount, CancellationToken cancellationToken);
}
