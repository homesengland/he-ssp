using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.Contract.Application.Commands;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;

public class StartApplicationCommandHandler : IRequestHandler<StartApplicationCommand, OperationResult<LoanApplicationId?>>
{
    private readonly IAccountUserContext _loanUserContext;
    private readonly ILoanApplicationRepository _applicationRepository;
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILogger<StartApplicationCommandHandler> _logger;

    public StartApplicationCommandHandler(
        IAccountUserContext loanUserContext,
        ILoanApplicationRepository applicationRepository,
        ILogger<StartApplicationCommandHandler> logger,
        IApplicationProjectsRepository applicationProjectsRepository)
    {
        _loanUserContext = loanUserContext;
        _applicationRepository = applicationRepository;
        _logger = logger;
        _applicationProjectsRepository = applicationProjectsRepository;
    }

    public async Task<OperationResult<LoanApplicationId?>> Handle(StartApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userAccount = await _loanUserContext.GetSelectedAccount();

            var applicationName = new LoanApplicationName(request.ApplicationName);
            var newLoanApplication = LoanApplicationEntity.New(userAccount, applicationName);

            if (await _applicationRepository.IsExist(applicationName, userAccount, cancellationToken))
            {
                return new OperationResult<LoanApplicationId?>(
                    new[] { new ErrorItem(nameof(LoanApplicationName), "This name has been used for another application") },
                    null);
            }

            await _applicationRepository.Save(newLoanApplication, await _loanUserContext.GetProfileDetails(), cancellationToken);

            var applicationProjects = new ApplicationProjects(newLoanApplication.Id);
            await _applicationProjectsRepository.SaveAllAsync(applicationProjects, userAccount, cancellationToken);

            await _applicationRepository.DispatchEvents(newLoanApplication, cancellationToken);

            return OperationResult.Success<LoanApplicationId?>(newLoanApplication.Id);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);

            return new OperationResult<LoanApplicationId?>(domainValidationException.OperationResult.Errors, null);
        }
    }
}
