using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class WithdrawLoanApplicationCommandHandler : IRequestHandler<WithdrawLoanApplicationCommand, OperationResult>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    public WithdrawLoanApplicationCommandHandler(ILoanApplicationRepository loanApplicationRepository)
    {
        _loanApplicationRepository = loanApplicationRepository;
    }

    public async Task<OperationResult> Handle(WithdrawLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var withdrawReason = WithdrawReason.New(request.WithdrawReason);
            var applicationStatus = (ApplicationStatus)Enum.Parse(typeof(ApplicationStatus), request.ApplicationStatus);
            var newApplicationStatus = applicationStatus == ApplicationStatus.ApplicationSubmitted ? ApplicationStatus.Withdrawn : ApplicationStatus.NA;
            await _loanApplicationRepository.Withdraw(request.LoanApplicationId, withdrawReason, newApplicationStatus, cancellationToken);

            return OperationResult.Success();
        }
        catch (DomainValidationException domainValidationException)
        {
            return domainValidationException.OperationResult;
        }
    }
}
