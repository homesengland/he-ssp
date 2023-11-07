using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class ProjectCommandHandlerBase
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    private readonly ILogger<ProjectCommandHandlerBase> _logger;

    public ProjectCommandHandlerBase(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
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
