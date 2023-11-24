using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories;

public interface IApplicationProjectsRepository
{
    Task SaveAsync(LoanApplicationId loanApplicationId, Project projectToSave, CancellationToken cancellationToken);

    Task<ApplicationProjects> GetAllAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<Project> GetByIdAsync(ProjectId projectId, UserAccount userAccount, ProjectFieldsSet projectFieldsSet, CancellationToken cancellationToken);

    Task SaveAllAsync(ApplicationProjects applicationProjects, UserAccount userAccount, CancellationToken cancellationToken);
}
