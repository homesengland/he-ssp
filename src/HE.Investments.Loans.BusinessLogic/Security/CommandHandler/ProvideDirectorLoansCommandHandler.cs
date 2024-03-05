using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Contract.Security.Commands;
using HE.Investments.Loans.Contract.Security.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Security.CommandHandler;

public class ProvideDirectorLoansCommandHandler : SecurityBaseCommandHandler, IRequestHandler<ProvideDirectorLoansCommand, OperationResult>
{
    public ProvideDirectorLoansCommandHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(securityRepository, loanApplicationRepository, loanUserContext)
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
