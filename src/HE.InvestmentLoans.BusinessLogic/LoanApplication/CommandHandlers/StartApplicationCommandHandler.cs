using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class StartApplicationCommandHandler : IRequestHandler<StartApplicationCommand, OperationResult<LoanApplicationId?>>
{
    private readonly ILoanUserContext _loanUserContext;

    private readonly ILoanApplicationRepository _applicationRepository;

    private readonly ILogger<StartApplicationCommandHandler> _logger;

    public StartApplicationCommandHandler(
        ILoanUserContext loanUserContext,
        ILoanApplicationRepository applicationRepository,
        ILogger<StartApplicationCommandHandler> logger)
    {
        _loanUserContext = loanUserContext;
        _applicationRepository = applicationRepository;
        _logger = logger;
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

            await _applicationRepository.Save(newLoanApplication, await _loanUserContext.GetUserDetails(), cancellationToken);

            return OperationResult.Success<LoanApplicationId?>(newLoanApplication.Id);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);

            return new OperationResult<LoanApplicationId?>(domainValidationException.OperationResult.Errors, null);
        }
    }
}
