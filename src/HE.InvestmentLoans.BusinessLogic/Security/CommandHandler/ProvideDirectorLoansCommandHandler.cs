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

public class ProvideDirectorLoansCommandHandler : SecurityBaseCommandHandler, IRequestHandler<ProvideDirectorLoansCommand, OperationResult>
{
    public ProvideDirectorLoansCommandHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
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
