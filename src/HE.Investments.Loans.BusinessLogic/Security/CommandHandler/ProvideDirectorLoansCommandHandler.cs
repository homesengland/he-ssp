using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Contract.Security.Commands;
using HE.Investments.Loans.Contract.Security.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Security.CommandHandler;

public class ProvideDirectorLoansCommandHandler : SecurityBaseCommandHandler, IRequestHandler<ProvideDirectorLoansCommand, OperationResult>
{
    public ProvideDirectorLoansCommandHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<SecurityBaseCommandHandler> logger)
        : base(securityRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideDirectorLoansCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            security =>
            {
                var directorLoans = request.Exists.IsProvided() ? DirectorLoans.FromString(request.Exists) : null;

                security.ProvideDirectorLoans(directorLoans!);
            },
            request.Id,
            cancellationToken);
    }
}
