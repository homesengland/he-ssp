using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Funding.Commands;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Funding.CommandHandlers;

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
