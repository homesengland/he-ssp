using HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class WithdrawLoanApplicationCommandHandler : IRequestHandler<WithdrawLoanApplicationCommand, OperationResult>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly IAccountUserContext _loanUserContext;
    private readonly ILogger<CompanyStructureBaseCommandHandler> _logger;

    public WithdrawLoanApplicationCommandHandler(
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<CompanyStructureBaseCommandHandler> logger)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _logger = logger;
    }

    public async Task<OperationResult> Handle(WithdrawLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loanApplication = await _loanApplicationRepository
                                .GetLoanApplication(request.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);
            var withdrawReason = WithdrawReason.New(request.WithdrawReason);

            await loanApplication.Withdraw(_loanApplicationRepository, withdrawReason, cancellationToken);
            await _loanApplicationRepository.DispatchEvents(loanApplication, cancellationToken);
            return OperationResult.Success();
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }
    }
}
