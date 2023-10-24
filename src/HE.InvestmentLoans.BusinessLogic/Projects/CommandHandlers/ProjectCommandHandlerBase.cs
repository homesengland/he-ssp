using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProjectCommandHandlerBase
{
    private readonly IApplicationProjectsRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    private readonly ILogger<ProjectCommandHandlerBase> _logger;

    public ProjectCommandHandlerBase(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
        _logger = logger;
    }

    protected async Task<OperationResult> Perform(Action<Project> action, LoanApplicationId loanApplicationId, ProjectId projectId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();

        var project = await _repository.GetByIdAsync(projectId, userAccount, ProjectFieldsSet.GetAllFields, cancellationToken)
            ?? throw new NotFoundException(nameof(ApplicationProjects), loanApplicationId);

        try
        {
            action(project);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);

            return domainValidationException.OperationResult;
        }

        await _repository.SaveAsync(loanApplicationId, project, cancellationToken);
        return OperationResult.Success();
    }
}
