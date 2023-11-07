using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;

public class ProvideAbnormalCostsCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideAbnormalCostsCommand, OperationResult>
{
    public ProvideAbnormalCostsCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext, ILogger<FundingBaseCommandHandler> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideAbnormalCostsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var abnormalCosts = request.IsAnyAbnormalCost.IsProvided() ? AbnormalCosts.FromString(request.IsAnyAbnormalCost!, request.AbnormalCostsAdditionalInformation) : null;

                x.ProvideAbnormalCosts(abnormalCosts);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
