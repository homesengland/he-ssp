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
internal class ProvideCompanyDebentureHandler : SecurityBaseCommandHandler, IRequestHandler<ProvideCompanyDebenture, OperationResult>
{
    public ProvideCompanyDebentureHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<SecurityBaseCommandHandler> logger)
        : base(securityRepository, loanApplicationRepository, loanUserContext, logger)
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
