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

public class ProvideAbnormalCostsCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideAbnormalCostsCommand, OperationResult>
{
    public ProvideAbnormalCostsCommandHandler(
        IFundingRepository fundingRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
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
