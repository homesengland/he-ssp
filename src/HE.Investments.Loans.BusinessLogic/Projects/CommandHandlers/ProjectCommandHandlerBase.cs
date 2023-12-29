using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProjectCommandHandlerBase
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    private readonly ILogger<ProjectCommandHandlerBase> _logger;

    public ProjectCommandHandlerBase(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _logger = logger;
    }

    protected async Task<OperationResult> Perform(
        Action<Project> action,
        LoanApplicationId loanApplicationId,
        ProjectId projectId,
        CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();

        var project = await _applicationProjectsRepository.GetByIdAsync(projectId, userAccount, ProjectFieldsSet.GetAllFields, cancellationToken)
                      ?? throw new NotFoundException(nameof(ApplicationProjects), loanApplicationId);

        try
        {
            action(project);

            if (project.LoanApplicationStatus != ApplicationStatus.Draft)
            {
                project.Publish(new LoanApplicationChangeToDraftStatusEvent(loanApplicationId));
                await _loanApplicationRepository.DispatchEvents(project, cancellationToken);
            }
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);

            return domainValidationException.OperationResult;
        }

        await _applicationProjectsRepository.SaveAsync(loanApplicationId, project, cancellationToken);
        return OperationResult.Success();
    }
}
