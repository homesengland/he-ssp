using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.InvestmentLoans.Contract.Application.Extensions;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class CheckProjectAnswersCommandHandler : IRequestHandler<CheckProjectAnswersCommand, OperationResult>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    private readonly ILogger<ProjectCommandHandlerBase> _logger;

    public CheckProjectAnswersCommandHandler(
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
                project.Publish(new LoanApplicationSectionHasBeenCompletedAgainEvent());
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
