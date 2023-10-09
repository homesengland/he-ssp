using System.Threading;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;

public interface IApplicationProjectsRepository
{
    public ApplicationProjects GetAll(LoanApplicationId loanApplicationId, UserAccount userAccount);

    public Task<Project> GetById(LoanApplicationId loanApplicationId, ProjectId projectId, UserAccount userAccount, ProjectFieldsSet projectFieldsSet, CancellationToken cancellationToken);

    Task SaveAsync(ApplicationProjects applicationProjects, ProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken);

    public LoanApplicationViewModel LegacyDeleteProject(Guid loanApplicationId, Guid projectId);

    Task<ApplicationProjects> GetById(LoanApplicationId loanApplicationId, UserAccount userAccount, ProjectFieldsSet projectFieldsSet, CancellationToken cancellationToken);
}
