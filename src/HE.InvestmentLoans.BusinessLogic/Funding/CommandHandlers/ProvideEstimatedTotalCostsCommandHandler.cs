using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;

public class ProvideEstimatedTotalCostsCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideEstimatedTotalCostsCommand, OperationResult>
{
    public ProvideEstimatedTotalCostsCommandHandler(
        IFundingRepository fundingRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<FundingBaseCommandHandler> logger)
        : base(fundingRepository, loanApplicationRepository, loanUserContext, logger)
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
