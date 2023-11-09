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
internal class ProvideCompanyDebentureHandler : SecurityBaseCommandHandler, IRequestHandler<ProvideCompanyDebenture, OperationResult>
{
    public ProvideCompanyDebentureHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
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
