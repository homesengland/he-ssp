using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;

public class ProvideAbnormalCostsCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideAbnormalCostsCommand, OperationResult>
{
    public ProvideAbnormalCostsCommandHandler(
        IFundingRepository fundingRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
        ILogger<FundingBaseCommandHandler> logger)
        : base(fundingRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideAbnormalCostsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var abnormalCosts = request.IsAnyAbnormalCost.IsProvided()
                    ? AbnormalCosts.FromString(request.IsAnyAbnormalCost!, request.AbnormalCostsAdditionalInformation)
                    : null;

                x.ProvideAbnormalCosts(abnormalCosts);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
