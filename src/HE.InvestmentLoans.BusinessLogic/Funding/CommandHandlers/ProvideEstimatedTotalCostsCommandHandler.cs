using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;

public class ProvideEstimatedTotalCostsCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideEstimatedTotalCostsCommand, OperationResult>
{
    public ProvideEstimatedTotalCostsCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvideEstimatedTotalCostsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var estimatedTotalCosts = request.EstimatedTotalCosts.IsProvided() ? EstimatedTotalCosts.FromString(request.EstimatedTotalCosts!) : null;
                x.ProvideEstimatedTotalCosts(estimatedTotalCosts);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
