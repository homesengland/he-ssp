using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Security.Commands;
using HE.InvestmentLoans.Contract.Security.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;

internal class ProvideDirectorLoansSubordinateCommandHandler : SecurityBaseCommandHandler,
    IRequestHandler<ProvideDirectorLoansSubordinateCommand, OperationResult>
{
    public ProvideDirectorLoansSubordinateCommandHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
        ILogger<SecurityBaseCommandHandler> logger)
        : base(securityRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideDirectorLoansSubordinateCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            security =>
            {
                var directorLoansSubordinate = request.CanBeSubordinated.IsProvided()
                    ? DirectorLoansSubordinate.FromString(request.CanBeSubordinated, request.ReasonWhyCannotBeSubordinated)
                    : null;

                security.ProvideDirectorLoansSubordinate(directorLoansSubordinate!);
            },
            request.Id,
            cancellationToken);
    }
}
