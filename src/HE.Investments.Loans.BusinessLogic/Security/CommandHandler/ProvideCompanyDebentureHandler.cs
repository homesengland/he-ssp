using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Contract.Security.Commands;
using HE.Investments.Loans.Contract.Security.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Security.CommandHandler;
internal class ProvideCompanyDebentureHandler : SecurityBaseCommandHandler, IRequestHandler<ProvideCompanyDebenture, OperationResult>
{
    public ProvideCompanyDebentureHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(securityRepository, loanApplicationRepository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvideCompanyDebenture request, CancellationToken cancellationToken)
    {
        return await Perform(
            security =>
            {
                var debenture = request.Exists.IsProvided() ? Debenture.FromString(request.Exists, request.Holder) : null;

                security.ProvideDebenture(debenture!);
            },
            request.Id,
            cancellationToken);
    }
}
