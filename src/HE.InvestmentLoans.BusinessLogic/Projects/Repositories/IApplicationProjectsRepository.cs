using System.Threading;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;

public interface IApplicationProjectsRepository
{
    Task SaveAsync(ApplicationProjects applicationProjects, ProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ApplicationProjects> GetById(LoanApplicationId loanApplicationId, UserAccount userAccount, ProjectFieldsSet projectFieldsSet, CancellationToken cancellationToken);

    Task SaveAllAsync(ApplicationProjects applicationProjects, UserAccount userAccount, CancellationToken cancellationToken);
}
