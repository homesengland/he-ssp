using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Contract.Security.Commands;
using HE.Investments.Loans.Contract.Security.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Security.CommandHandler;

internal class ProvideDirectorLoansSubordinateCommandHandler : SecurityBaseCommandHandler,
    IRequestHandler<ProvideDirectorLoansSubordinateCommand, OperationResult>
{
    public ProvideDirectorLoansSubordinateCommandHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
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
