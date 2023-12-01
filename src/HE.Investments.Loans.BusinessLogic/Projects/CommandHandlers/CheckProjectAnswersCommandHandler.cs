using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.Extensions;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
public class CheckProjectAnswersCommandHandler : IRequestHandler<CheckProjectAnswersCommand, OperationResult>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    private readonly ILogger<ProjectCommandHandlerBase> _logger;

    public CheckProjectAnswersCommandHandler(
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

    public async Task<OperationResult> Handle(CheckProjectAnswersCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var project = await _applicationProjectsRepository.GetByIdAsync(request.ProjectId, userAccount, ProjectFieldsSet.GetAllFields, cancellationToken)
                      ?? throw new NotFoundException(nameof(ApplicationProjects), request.LoanApplicationId);
        try
        {
            project.CheckAnswers(request.YesNoAnswer.ToYesNoAnswer());

            if (project.Status == SectionStatus.Completed)
            {
                project.Publish(new LoanApplicationSectionHasBeenCompletedAgainEvent(request.LoanApplicationId));
                await _loanApplicationRepository.DispatchEvents(project, cancellationToken);
            }
            else
            {
                project.Publish(new LoanApplicationChangeToDraftStatusEvent(request.LoanApplicationId));
                await _loanApplicationRepository.DispatchEvents(project, cancellationToken);
            }
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _applicationProjectsRepository.SaveAsync(request.LoanApplicationId, project, cancellationToken);
        return OperationResult.Success();
    }
}
