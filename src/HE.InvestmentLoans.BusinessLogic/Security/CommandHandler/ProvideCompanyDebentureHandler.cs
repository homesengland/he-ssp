using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Security.Commands;
using HE.InvestmentLoans.Contract.Security.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
internal class ProvideCompanyDebentureHandler : SecurityBaseCommandHandler, IRequestHandler<ProvideCompanyDebenture, OperationResult>
{
    public ProvideCompanyDebentureHandler(ISecurityRepository securityRepository, ILoanUserContext loanUserContext)
        : base(securityRepository, loanUserContext)
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
