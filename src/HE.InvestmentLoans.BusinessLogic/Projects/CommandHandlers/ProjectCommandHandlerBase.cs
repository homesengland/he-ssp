using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProjectCommandHandlerBase
{
    private readonly IApplicationProjectsRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    public ProjectCommandHandlerBase(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
    }

    protected async Task<OperationResult> Perform(Action<Project> action, LoanApplicationId loanApplicationId, ProjectId projectId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();

        var applicationProjects = await _repository.GetById(loanApplicationId, userAccount, cancellationToken)
            ?? throw new NotFoundException(nameof(ApplicationProjects), loanApplicationId);

        var project = applicationProjects.Projects.FirstOrDefault(p => p.Id == projectId)
            ?? throw new NotFoundException(nameof(Project), projectId);

        try
        {
            action(project);
        }
        catch (DomainValidationException domainValidationException)
        {
            return domainValidationException.OperationResult;
        }

        await _repository.SaveAsync(applicationProjects, projectId, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
