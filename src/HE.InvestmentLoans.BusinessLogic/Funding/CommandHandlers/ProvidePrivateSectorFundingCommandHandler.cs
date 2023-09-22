using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;

public class ProvidePrivateSectorFundingCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvidePrivateSectorFundingCommand, OperationResult>
{
    public ProvidePrivateSectorFundingCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvidePrivateSectorFundingCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var privateSectorFunding = request.IsApplied.IsProvided() ? PrivateSectorFunding.FromString(request.IsApplied!, request.PrivateSectorFundingApplyResult, request.PrivateSectorFundingNotApplyingReason) : null;

                x.ProvidePrivateSectorFunding(privateSectorFunding);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
